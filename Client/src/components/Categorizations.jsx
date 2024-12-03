import { useState } from "react";
import { Link, useLoaderData, useRouteLoaderData } from "react-router-dom";
import Transaction from "./Transaction";

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
          uncategorized transactions
        </p>
        <Link to="/home/categorizations" className="text-pastelDarkGreen">
          Categorize transactions
        </Link>
      </div>
    )
  ) : (
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
  );
};

export default Categorizations;
