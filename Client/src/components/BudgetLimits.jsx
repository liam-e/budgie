import { useState, useMemo, useEffect } from "react";
import { Link, useLoaderData, useRouteLoaderData } from "react-router-dom";
import BudgetLimit from "./BudgetLimit";
import BudgetLimitForm from "../forms/BudgetLimitForm";
import {
  filterTransactionsByDateRange,
  getDateRangesForPeriod,
} from "../utils/dates";
import { sumByCategory } from "../utils/calc";

const BudgetLimits = ({ periodType, isOnDashboard = false }) => {
  const [budgetLimits, setBudgetLimits] = useState([]);
  const { mapCategoryIdToName } = useRouteLoaderData("home");
  const transactions = useLoaderData();

  useEffect(() => {
    // TODO: Improve use of loader so this isn't needed
    const getBudgetLimits = async () => {
      try {
        const response = await fetch(
          `${import.meta.env.VITE_API_URL}/BudgetLimits`,
          {
            method: "GET",
            credentials: "include",
            headers: {
              "Content-Type": "application/json",
            },
          }
        );

        if (!response.ok) {
          throw new Error(
            response.statusText || "Failed to fetch budget limits"
          );
        }

        const data = await response.json();
        return data;
      } catch (error) {
        console.error("Error fetching budget limits:", error);
      }
    };
    getBudgetLimits()
      .then((data) => {
        setBudgetLimits(data);
        return data;
      })
      .catch((error) => console.error(error));
  }, []);

  const updateBudgetLimits = (data) => {
    setBudgetLimits([...budgetLimits, data]);
  };

  const filteredLimits = useMemo(() => {
    return budgetLimits.filter((bl) => bl.periodType === periodType);
  }, [budgetLimits, periodType]);

  const transactionSumsByCategory = useMemo(() => {
    const { startDates, endDates } = getDateRangesForPeriod(periodType);
    return sumByCategory(
      filterTransactionsByDateRange(transactions, startDates, endDates)
    );
  }, [transactions, periodType]);

  return (
    <div className="flex flex-col space-y-4">
      {/* BUDGET LIMIT FORM */}
      {!isOnDashboard && (
        <BudgetLimitForm
          periodType={periodType}
          updateBudgetLimits={updateBudgetLimits}
        />
      )}
      <div className="flex flex-col space-y-4">
        <h4 className="capitalize">{periodType + " Limits"}</h4>
        {filteredLimits.length === 0 ? (
          <div className="text-gray-400">(No budget limits)</div>
        ) : (
          filteredLimits.map((limit, index) => {
            const categorySum = transactionSumsByCategory.find(
              (c) => c.categoryId === limit.categoryId
            );
            return (
              <BudgetLimit
                key={index}
                limit={{
                  ...limit,
                  categoryName: mapCategoryIdToName[limit.categoryId],
                  actualAmount: categorySum ? categorySum.amount : 0,
                }}
              />
            );
          })
        )}
      </div>
      {isOnDashboard && (
        <Link to="/home/budget-limits" className="text-pastelDarkGreen">
          Set Budget Limits
        </Link>
      )}
    </div>
  );
};

export default BudgetLimits;
