import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import CurrencyInputControl from "../controls/CurrencyInputControl";
import CategorySelectorControl from "../controls/CategorySelectorControl";
import InputErrorMessage from "../components/InputErrorMessage";
import LinkButton from "../components/LinkButton";
import { useRouteLoaderData } from "react-router-dom";
import { message } from "../components/MessageContainer";

const TransactionForm = ({ transaction, edit = false }) => {
  const { categories } = useRouteLoaderData("home");

  const {
    register,
    handleSubmit,
    control,
    watch,
    reset,
    formState: { errors },
  } = useForm({
    defaultValues: edit
      ? {
          date: transaction.date || "",
          description: transaction.description || "",
          amount: Math.abs(transaction.amount).toString() || "",
          category: {
            id: transaction.categoryId || "",
            name: transaction.categoryName || "",
            transactionTypeId:
              categories.find((c) => c.id === transaction.categoryId)
                ?.transactionTypeId || "",
          },
        }
      : {},
  });

  const watchDate = watch("date");
  const watchCategory = watch("category");

  const getErrorMessage = (errors) => {
    if (errors) {
      if (errors.date) return errors.date.message;
      if (errors.description) return errors.description.message;
      if (errors.category) return "Please select a category";
      if (errors.amount) return "Please enter the amount";
    }
    return "";
  };

  const onSubmit = (data) => {
    edit
      ? handleEditTransaction(data).then(() => {
          message("Transaction updated successfully.");
        })
      : handleAddTransaction(data).then((data) => {
          reset();
          message("Trnasaction added successfully.");
        });
  };

  const handleAddTransaction = async (data) => {
    const newTransaction = {
      date: data.date,
      description: data.description,
      amount:
        data.category.transactionTypeId === "expense"
          ? -Math.abs(Number(data.amount.replace(",", "")))
          : Math.abs(Number(data.amount.replace(",", ""))),
      currency: "AUD",
      categoryId: data.category.id,
    };

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Transactions`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(newTransaction),
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        console.error(
          `Failed to create transaction: ${
            errorData.error || response.statusText
          }`
        );
        return;
      }
    } catch (error) {
      console.error("Error adding transaction:", error);
      return;
    }
  };

  const handleEditTransaction = async (data) => {
    const updatedTransaction = {
      id: transaction.id,
      userId: transaction.userId,
      date: data.date,
      description: data.description,
      amount:
        data.category.transactionTypeId === "expense"
          ? -Math.abs(Number(data.amount.replace(",", "")))
          : Math.abs(Number(data.amount.replace(",", ""))),
      currency: "AUD",
      categoryId: data.category.id,
    };

    console.log("data:", JSON.stringify(data));
    console.log("updated:", JSON.stringify(updatedTransaction));

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Transactions/${transaction.id}`,
        {
          method: "PUT",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(updatedTransaction),
        }
      );

      if (response.ok) {
        console.log("Transaction updated successfully.");
      } else {
        const errorData = await response.json();
        console.error("Error updating transaction:", errorData);
        throw Error("Error updating transaction:", errorData);
      }
    } catch (error) {
      console.error("An unexpected error occurred:", error);
      throw error;
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-row content-between">
        <div className="flex flex-col p-2 space-y-4 basis-2/4">
          <input
            type="date"
            placeholder="Select a date"
            {...register("date", {
              required: {
                value: true,
                message: "Please enter the transaction date",
              },
            })}
            aria-invalid={!!errors?.date}
            className={`${!watchDate ? "text-gray-400" : "text-black"} ${
              errors.date ? "invalid" : ""
            }`}
          />

          <CategorySelectorControl
            control={control}
            name="category"
            rules={{
              required: {
                value: true,
                message: "Please select a category",
              },
            }}
            isInvalid={!!errors?.category}
          />
        </div>

        <div className="flex flex-col p-2 space-y-4 basis-2/4">
          <input
            type="text"
            placeholder="Description"
            {...register("description", {
              required: {
                value: true,
                message: "Please enter the transaction description",
              },
            })}
            className={errors.description ? "invalid" : ""}
          />

          <CurrencyInputControl
            control={control}
            name="amount"
            rules={{
              required: {
                value: true,
                message: "Please enter the amount",
              },
            }}
            isInvalid={!!errors?.amount}
          />
        </div>
      </div>
      <div className="text-sm text-center">
        {watchCategory ? (
          <p className="text-black">
            This transaction will be recorded as{" "}
            {watchCategory.transactionTypeId === "expense"
              ? "an Expense (-)"
              : "an Income (+)"}
            .
          </p>
        ) : (
          <p className="invisible">[Placeholder]</p>
        )}
      </div>

      <div className="flex flex-col items-center space-y-2">
        <InputErrorMessage>{getErrorMessage(errors)}</InputErrorMessage>

        <div className="flex space-x-4">
          <ButtonComponent type="submit">
            {edit ? "Edit" : "Add"}
          </ButtonComponent>
          <LinkButton to="/home/dashboard">Back to dashboard</LinkButton>
        </div>
      </div>
    </form>
  );
};

export default TransactionForm;
