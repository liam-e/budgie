import { useState, useMemo, useEffect } from "react";
import { useLoaderData, useRouteLoaderData } from "react-router-dom";
import BudgetLimit from "./BudgetLimit";
import BudgetLimitForm from "../forms/BudgetLimitForm";
import {
  filterTransactionsByDateRange,
  getDateRangesForPeriod,
} from "../utils/dates";
import { sumByCategory } from "../utils/calc";
import LinkComponent from "./LinkComponent";

const BudgetLimits = ({ periodType, offset, isOnDashboard = false }) => {
  const [budgetLimits, setBudgetLimits] = useState([]);
  const { mapCategoryIdToName } = useRouteLoaderData("home");
  const transactions = useLoaderData();

  useEffect(() => {
    (async () => {
      try {
        const response = await fetch(
          `${import.meta.env.VITE_API_URL}/BudgetLimits`,
          { method: "GET", credentials: "include" }
        );

        if (!response.ok) {
          throw new Error(
            response.statusText || "Failed to fetch budget limits"
          );
        }

        const data = await response.json();
        setBudgetLimits(data);
      } catch (error) {
        console.error("Error fetching budget limits:", error);
      }
    })();
  }, []);

  const onUpdateBudgetLimit = (newLimit) => {
    setBudgetLimits((prev) => [...prev, newLimit]);
  };

  const onDeleteBudgetLimit = (id) => {
    setBudgetLimits((prev) => prev.filter((limit) => limit.id !== id));
  };

  const filteredLimits = useMemo(() => {
    if (isOnDashboard) {
      return budgetLimits
        .sort((a, b) => Math.abs(b.amount) - Math.abs(a.amount))
        .slice(0, 10)
        .filter((bl) => bl.periodType === periodType);
    } else {
      return budgetLimits
        .sort((a, b) => Math.abs(b.amount) - Math.abs(a.amount))
        .filter((bl) => bl.periodType === periodType);
    }
  }, [budgetLimits, periodType]);

  const transactionSumsByCategory = useMemo(() => {
    const { startDates, endDates } = getDateRangesForPeriod(periodType, offset);
    return sumByCategory(
      filterTransactionsByDateRange(transactions, startDates, endDates)
    );
  }, [transactions, periodType, offset]);

  return (
    <div className="flex flex-col space-y-4">
      {/* Budget Limit Form */}
      {!isOnDashboard && (
        <BudgetLimitForm
          periodType={periodType}
          updateBudgetLimits={onUpdateBudgetLimit}
          categoriesTaken={new Set(budgetLimits.map((bl) => bl.categoryId))}
        />
      )}

      <div className="flex flex-col space-y-4 min-w-fit">
        <h4 className="capitalize text-lg">{periodType} Limits</h4>
        {filteredLimits.length === 0 ? (
          <div className="emptymessage">(No budget limits)</div>
        ) : (
          filteredLimits.map((limit) => {
            const categorySum =
              transactionSumsByCategory.find(
                (c) => c.categoryId === limit.categoryId
              ) || {};
            return (
              <BudgetLimit
                key={limit.id}
                limit={{
                  ...limit,
                  categoryName:
                    mapCategoryIdToName[limit.categoryId] || limit.categoryId,
                  actualAmount: categorySum.amount || 0,
                }}
                updateBudgetLimits={onDeleteBudgetLimit}
                isOnDashboard={isOnDashboard}
              />
            );
          })
        )}
      </div>

      {isOnDashboard && (
        <LinkComponent to="/home/budget-limits">
          Set Budget Limits
        </LinkComponent>
      )}
    </div>
  );
};

export default BudgetLimits;
