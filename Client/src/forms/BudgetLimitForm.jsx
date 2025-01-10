import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import CategorySelectorControl from "../controls/CategorySelectorControl";
import CurrencyInputControl from "../controls/CurrencyInputControl";

const BudgetLimitForm = ({
  periodType,
  updateBudgetLimits,
  categoriesTaken,
}) => {
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
    reset,
    watch,
  } = useForm();

  const watchCategory = watch("category");

  const onSubmit = async (data) => {
    handleAddTransaction(data).then(reset);
  };

  const handleAddTransaction = async (data) => {
    try {
      const limit = {
        categoryId: data.category.id,
        periodType,
        amount:
          data.category.transactionTypeId === "expense"
            ? -Math.abs(parseFloat(data.amount.replace(",", "")))
            : Math.abs(parseFloat(data.amount.replace(",", ""))),
      };

      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/BudgetLimits`,
        {
          method: "POST",
          credentials: "include",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(limit),
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        console.log(errorData);
        console.error(
          `Failed to create budget limit: ${
            errorData.error || response.statusText
          }`
        );
        return;
      }

      const newLimit = await response.json();
      updateBudgetLimits(newLimit);
    } catch (error) {
      console.error("Error adding budget limit:", error);
    }
  };

  return (
    <form className="flex flex-col space-y-2" onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-row items-center space-x-4">
        <CategorySelectorControl
          control={control}
          name="category"
          rules={{ required: "Category is required" }}
          isInvalid={!!errors?.category}
          categoriesToExclude={categoriesTaken}
        />
        <CurrencyInputControl
          control={control}
          name="amount"
          rules={{ required: "Amount is required" }}
          isInvalid={!!errors?.amount}
        />
        <ButtonComponent type="submit">Add</ButtonComponent>
      </div>

      <div className="text-sm text-center">
        {watchCategory ? (
          <p className="text-black">
            This amount will be recorded as{" "}
            {watchCategory.transactionTypeId === "expense"
              ? "an Expense (-)"
              : "an Income (+)"}
            .
          </p>
        ) : (
          <p className="invisible">[Placeholder]</p>
        )}
      </div>
    </form>
  );
};

export default BudgetLimitForm;
