import React from "react";
import Category from "./Category";
import { formatCurrency } from "../utils/format";
import { useRouteLoaderData } from "react-router-dom";

const TopExpenses = ({ categoryAmounts }) => {
  const { mapCategoryIdToName } = useRouteLoaderData("home");
  const filteredCategories = categoryAmounts
    .filter((c) => c.amount > 0)
    .slice(0, 3);

  if (filteredCategories.length === 0) return null;

  return (
    <div className="flex flex-col space-y-5">
      <h4>Top Expenses</h4>
      {filteredCategories.map((c, idx) => (
        <div
          key={idx}
          className="flex flex-row items-center space-x-5 p-2 text-sm bg-gray-200"
        >
          <div>
            <Category
              id={c.categoryId}
              name={mapCategoryIdToName[c.categoryId]}
              displayName={true}
            />
          </div>
          <div>{c[0]}</div>
          <div className="font-bold grow text-right text-red-500">
            {formatCurrency(-c.amount)}
          </div>
        </div>
      ))}
    </div>
  );
};

export default TopExpenses;
