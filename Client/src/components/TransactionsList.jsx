import { Link, useRouteLoaderData } from "react-router-dom";
import Transaction from "../components/Transaction";

const TransactionsList = ({ transactions }) => {
  const { mapCategoryIdToName } = useRouteLoaderData("home");

  return (
    <div className="flex flex-col">
      {transactions &&
        transactions.map((transaction, idx) => (
          <div key={idx}>
            <Link
              to={`/home/transactions/${transaction.id}`}
              state={{ transaction }}
            >
              <Transaction
                transaction={{
                  ...transaction,
                  categoryName: mapCategoryIdToName[transaction.categoryId],
                }}
                idx={idx}
              />
            </Link>
          </div>
        ))}
    </div>
  );
};

export default TransactionsList;
