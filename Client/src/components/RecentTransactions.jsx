import { useLoaderData } from "react-router-dom";
import TransactionsList from "./TransactionsList";

const RecentTransactions = () => {
  const transactions = useLoaderData().slice(0, 5);
  return (
    <div className="flex flex-col space-y-5">
      <h3>Recent transactions</h3>
      <TransactionsList transactions={transactions} />
      <a href="/home/transactions">View all transactions</a>
    </div>
  );
};

export default RecentTransactions;
