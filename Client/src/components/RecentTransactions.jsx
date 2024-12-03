import { Link, useLoaderData } from "react-router-dom";
import TransactionsList from "./TransactionsList";

const RecentTransactions = ({ isOnDashboard = false }) => {
  const transactions = useLoaderData().slice(0, 5);
  return (
    <div className="flex flex-col space-y-5 my-2">
      <h3>Recent transactions</h3>
      {transactions && transactions.length > 0 ? (
        <>
          <TransactionsList
            transactions={transactions}
            isOnDashboard={isOnDashboard}
          />
          {!isOnDashboard && (
            <div className="flex flex-row space-x-5">
              <Link to="/home/transactions" className="text-pastelDarkGreen">
                View all transactions
              </Link>
              <Link to="/home/add-data" className="text-pastelDarkGreen">
                Add Data
              </Link>
            </div>
          )}
        </>
      ) : (
        <p>
          There are no transactions to show.{" "}
          <Link to="/home/add-data">Add data</Link>
        </p>
      )}
    </div>
  );
};

export default RecentTransactions;
