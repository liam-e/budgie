import React from "react";
import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import CurrencyInputControl from "../controls/CurrencyInputControl";
import CategorySelectorControl from "../controls/CategorySelectorControl";
import InputErrorMessage from "../components/InputErrorMessage";
import LinkButton from "../components/LinkButton";

const ManualEntryForm = () => {
  const {
    register,
    handleSubmit,
    control,
    watch,
    reset,
    formState: { errors },
  } = useForm();

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
    handleAddTransaction(data).then(reset);
  };

  const handleAddTransaction = async (data) => {
    const isExpense =
      data.category.transactionTypeId === "purchase" ||
      data.category.transactionTypeId === "payment";

    const transaction = {
      date: data.date,
      description: data.description,
      amount: isExpense
        ? -Math.abs(parseFloat(data.amount.replace(",", "")))
        : Math.abs(parseFloat(data.amount.replace(",", ""))),
      currency: "AUD",
      categoryId: data.category.id,
    };

    console.log(transaction.amount);

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Transactions`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(transaction),
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        console.error("Error:", errorData);
        return null;
      }
    } catch (error) {
      console.error("Error:", error);
      return null;
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
            {watchCategory.transactionTypeId === "purchase" ||
            watchCategory.transactionTypeId === "payment"
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

        <div className="flex space-x-6">
          <ButtonComponent type="submit">Add</ButtonComponent>
          <LinkButton to="/home/dashboard">Back to dashboard</LinkButton>
        </div>
      </div>
    </form>
  );
};

export default ManualEntryForm;
