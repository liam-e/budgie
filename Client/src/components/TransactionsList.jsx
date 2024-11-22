import { useRouteLoaderData } from "react-router-dom";
import Transaction from "../components/Transaction";

const TransactionsList = ({ transactions }) => {
  const { mapCategoryIdToName } = useRouteLoaderData("home");

  return (
    <div className="flex flex-col space-y-4">
      <div>
        {transactions &&
          transactions.map((t, idx) => (
            <Transaction
              key={idx}
              idx={idx}
              data={{ ...t, categoryName: mapCategoryIdToName[t.categoryId] }}
            />
          ))}
      </div>
    </div>
  );
};

export default TransactionsList;
