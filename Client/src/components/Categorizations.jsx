import { useState } from "react";
import { useLoaderData, useRouteLoaderData } from "react-router-dom";
import Transaction from "./Transaction";
import LinkComponent from "./LinkComponent";

const Categorizations = ({ isOnDashboard = false }) => {
  const transactions = useLoaderData();
  const { mapCategoryIdToName } = useRouteLoaderData("home");

  const [uncategorized, setUncategorized] = useState(() =>
    transactions.filter((t) => t.categoryId === "none")
  );

  const handleTransactionCategorized = (transactionId) => {
    setUncategorized((prev) => prev.filter((t) => t.id !== transactionId));
  };

  return isOnDashboard ? (
    uncategorized.length > 0 && (
      <div className="flex flex-col space-y-4 p-6 bg-pastelYellow text-center my-4">
        <p className="text-black text-2xl">
          You have{" "}
          <span className="text-pastelDarkGreen font-bold">
            {uncategorized.length}{" "}
          </span>
          uncategorised transaction{uncategorized.length > 1 && "s"}
        </p>
        <LinkComponent to="/home/categorizations">
          Categorise transactions
        </LinkComponent>
      </div>
    )
  ) : uncategorized.length > 0 ? (
    <div className="flex flex-col">
      {uncategorized.map((t, idx) => (
        <Transaction
          transaction={t}
          key={idx}
          idx={idx}
          data={{ ...t, categoryName: mapCategoryIdToName[t.categoryId] }}
          categorize={true}
          onCategorized={() => handleTransactionCategorized(t.id)}
        />
      ))}
    </div>
  ) : (
    <p className="emptymessage">There are no transactions to categorise.</p>
  );
};

export default Categorizations;
