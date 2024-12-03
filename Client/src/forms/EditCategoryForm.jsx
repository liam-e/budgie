import { useForm } from "react-hook-form";
import CategorySelectorControl from "../controls/CategorySelectorControl";
import { useEffect } from "react";
import InputErrorMessage from "../components/InputErrorMessage";
import { message } from "../components/MessageContainer";

const EditCategoryForm = ({ transaction, onCategorized }) => {
  const {
    register,
    handleSubmit,
    control,
    watch,
    reset,
    setError,
    formState: { errors },
  } = useForm({ mode: "onSubmit" });

  useEffect(() => {
    const subscription = watch(handleSubmit(onSubmit));
    return () => subscription.unsubscribe();
  }, [handleSubmit, watch]);

  const onSubmit = (data) => {
    putTransaction(data).then((msg) => {
      message(msg);
      if (onCategorized) onCategorized(transaction.id);
    });
  };

  const putTransaction = async (data) => {
    const modifiedTransaction = {
      ...transaction,
      categoryId: data.category.id,
    };

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/transactions/${transaction.id}`,
        {
          method: "PUT",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(modifiedTransaction),
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        setError(
          "category",
          {
            type: "focus",
            message: errorData.error || "Failed to update category.",
          },
          { shouldFocus: true }
        );
        console.error(errorData.error || "Failed to update transaction.");
      }

      return `Category set to ${JSON.stringify(data.category.name)}`;
    } catch (error) {
      setError(
        "category",
        { type: "focus", message: error.message },
        { shouldFocus: true }
      );
      console.error("Error updating transaction:", error.message);
    }
  };

  return (
    <form className="flex flex-row">
      <CategorySelectorControl
        control={control}
        name="category"
        rules={{ required: true }}
        isInvalid={!!errors?.category}
        amountIsPositive={transaction.amount > 0}
      />
      {errors.category && (
        <InputErrorMessage>{errors.category.message}</InputErrorMessage>
      )}
    </form>
  );
};

export default EditCategoryForm;
