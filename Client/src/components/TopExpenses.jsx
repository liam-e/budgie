import React, { useEffect, useState } from "react";
import Category from "./Category";
import { formatCurrency } from "../utils/format";
import { useRouteLoaderData } from "react-router-dom";
import { sumByCategory } from "../utils/calc";

const TopExpenses = ({ transactions }) => {
  const [categoryAmounts, setCategoryAmounts] = useState([]);

  useEffect(() => {
    setCategoryAmounts(
      sumByCategory(transactions)
        .filter((c) => c.amount < 0)
        .reverse()
        .slice(0, 4)
    );
  }, [transactions]);

  const { mapCategoryIdToName } = useRouteLoaderData("home");

  return (
    <div className="flex flex-col space-y-5">
      <h3 className="text-lg">Top Expenses</h3>
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        {categoryAmounts.map((c, idx) => (
          <div
            key={idx}
            className="flex flex-col items-center align-center justify-between space-y-2 bg-gray-200 text-sm px-2 py-4"
          >
            {/* Category Section */}
            <Category
              id={c.categoryId}
              name={mapCategoryIdToName[c.categoryId] || c.categoryId}
              displayName={true}
            />

            {/* Expense Amount */}
            <div className="font-bold text-red-600">
              {formatCurrency(-c.amount)}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TopExpenses;
