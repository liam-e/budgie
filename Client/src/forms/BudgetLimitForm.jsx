import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import CategorySelectorControl from "../controls/CategorySelectorControl";
import CurrencyInputControl from "../controls/CurrencyInputControl";

const BudgetLimitForm = ({ periodType, updateBudgetLimits }) => {
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm();

  const onSubmit = (data) => {
    handleAddBudgetLimit(data)
      .then((newLimit) => {
        if (newLimit) {
          updateBudgetLimits(newLimit); // Add new limit to the state in BudgetLimits
        }
      })
      .catch((error) => {
        console.log("Error adding budget limit:", error);
      });
  };

  const handleAddBudgetLimit = async (data) => {
    const limit = {
      categoryId: data.category.id,
      periodType,
      amount: parseFloat(data.amount),
    };

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/BudgetLimits`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(limit),
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        console.error("Error:", errorData);
        alert(
          `Failed to create budget limit: ${
            errorData.error || response.statusText
          }`
        );
        return null;
      }

      const newLimit = await response.json();
      return newLimit;
    } catch (error) {
      console.error("Error:", error);
      return null;
    }
  };

  return (
    <form
      className="flex flex-row items-center space-x-4"
      onSubmit={handleSubmit(onSubmit)}
    >
      <CategorySelectorControl
        control={control}
        name="category"
        rules={{ required: true }}
        isInvalid={!!errors?.category}
      />
      <CurrencyInputControl
        control={control}
        name="amount"
        rules={{ required: true }}
        isInvalid={!!errors?.amount}
      />
      <ButtonComponent type="submit">Add</ButtonComponent>
    </form>
  );
};

export default BudgetLimitForm;
