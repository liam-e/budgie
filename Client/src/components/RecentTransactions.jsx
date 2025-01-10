import { useLoaderData } from "react-router-dom";
import TransactionsList from "./TransactionsList";
import LinkComponent from "./LinkComponent";

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
              <LinkComponent to="/home/transactions">
                View all transactions
              </LinkComponent>
              <LinkComponent to="/home/add-data">Add Data</LinkComponent>
            </div>
          )}
        </>
      ) : (
        <>
          <p className="text-gray-600">There are no transactions to show.</p>

          <LinkComponent to="/home/add-data">Add data</LinkComponent>
        </>
      )}
    </div>
  );
};

export default RecentTransactions;
