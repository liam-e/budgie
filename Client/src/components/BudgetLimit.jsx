import Category from "./Category";
import { formatCurrency } from "../utils/format";
import DeleteButton from "./DeleteButton";
import { useState } from "react";
import { FaTrash } from "react-icons/fa";
import ButtonSpinner from "./ButtonSpinner";
import { message } from "./MessageContainer";

const BudgetLimit = ({ limit, updateBudgetLimits, isOnDashboard = false }) => {
  const [isLoading, setIsLoading] = useState(false);

  const overUnder = limit.actualAmount - limit.amount;

  const handleDeleteBudgetLimit = async () => {
    try {
      setIsLoading(true);
      await deleteBudgetLimit(limit.id);
      updateBudgetLimits(limit.id);
      message(`Budget limit for "${limit.categoryName}" removed.`);
    } catch (error) {
      console.error("Error deleting budget limit:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const deleteBudgetLimit = async (id) => {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/BudgetLimits/${id}`,
      {
        method: "DELETE",
        credentials: "include",
      }
    );

    if (!response.ok) {
      const errorMessage =
        (await response.json())?.error || "Failed to delete budget limit.";
      throw new Error(errorMessage);
    }
  };

  return (
    <div className="flex flex-row items-center h-16 w-full space-x-3 p-2 bg-pastelYellow text-sm">
      {/* CATEGORY */}
      <div>
        <Category
          id={limit.categoryId}
          name={limit.categoryName}
          displayName={true}
        />
      </div>

      {/* LIMIT AMOUNT */}
      <div className="text-left font-bold">{formatCurrency(limit.amount)}</div>

      {/* ACTUAL AMOUNT */}
      <div className="grow w-16 text-right">
        <div className="font-bold">{formatCurrency(limit.actualAmount)}</div>

        {/* OVER/UNDER */}
        {limit.actualAmount !== 0 && (
          <div
            className={
              "font-bold " +
              ((limit.actualAmount > 0 && limit.amount > 0) || overUnder > 0
                ? "text-green-600"
                : "text-red-500")
            }
          >
            ({formatCurrency(Math.abs(overUnder))}{" "}
            {overUnder < 0 || limit.amount > 0 ? "over" : "under"})
          </div>
        )}
      </div>

      {/* DELETE BUTTON */}
      {isOnDashboard || (
        <DeleteButton onClick={handleDeleteBudgetLimit}>
          {isLoading ? (
            <ButtonSpinner />
          ) : (
            <div className="flex flex-row space-x-1 items-center">
              <FaTrash />
            </div>
          )}
        </DeleteButton>
      )}
    </div>
  );
};

export default BudgetLimit;
