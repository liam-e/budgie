import { useEffect, useState } from "react";
import { useLoaderData } from "react-router-dom";
import TransactionsList from "../components/TransactionsList";

const TransactionsPage = () => {
  const [limit, setLimit] = useState(20);
  const [page, setPage] = useState(0);
  const [transactionsToShow, setTransactionsToShow] = useState(null);

  useEffect(() => {
    console.log("This just got called!");
    handleSetTranactionsToShow();
  }, [limit, page]);

  const transactions = useLoaderData(); // TODO: Use this instead and pass in the transactions

  const handleChangeLimit = (e) => {
    e.preventDefault();
    setLimit(Number(e.target.value));
  };

  const handleChangePage = (e) => {
    e.preventDefault();
    setPage(Number(e.target.value));
  };

  const handleSetTranactionsToShow = () => {
    const start = page * limit;
    const end = page * limit + limit;

    console.log("start: ", start, " end: ", end, transactionsToShow);

    setTransactionsToShow(transactions.slice(start, end));
  };

  const pageSelector = (
    <div className="flex flex-row space-x-2 items-center">
      <label htmlFor="page-select">Page:</label>
      <select
        className="w-24 border-black border-2 p-1"
        name="page-select"
        id="page-select"
        value={page.toString()}
        onChange={(e) => {
          handleChangePage(e);
        }}
      >
        {/* TODO: Make programmatic */}
        <option value="1">1</option>
        <option value="2">2</option>
        <option value="3">3</option>
      </select>
    </div>
  );

  const limitSelector = (
    <div className="flex flex-row space-x-2 items-center">
      <label htmlFor="limit-select">Limit:</label>
      <select
        className="w-24 border-black border-2 p-1"
        name="limit-select"
        id="limit-select"
        value={limit.toString()}
        onChange={(e) => {
          handleChangeLimit(e);
        }}
      >
        <option value="10">10</option>
        <option value="20">20</option>
        <option value="50">50</option>
      </select>
    </div>
  );

  return (
    <div className="container mx-auto max-w-6xl flex flex-col space-y-5 p-6">
      <h2>Transactions</h2>
      <div className="flex flex-row items-center space-x-4">
        {pageSelector}

        {limitSelector}
      </div>
      <TransactionsList transactions={transactionsToShow} />
    </div>
  );
};

const transactionsLoader = async ({ params }) => {
  const response = await fetch(`${import.meta.env.VITE_API_URL}/transactions`, {
    method: "GET",
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (!response.ok) throw new Error(response.statusText);

  const data = await response.json();
  return data.transactions;
};

export { TransactionsPage as default, transactionsLoader };
