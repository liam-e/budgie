import { useEffect, useState } from "react";
import { Link, useLoaderData } from "react-router-dom";
import TransactionsList from "../components/TransactionsList";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";

const TransactionsPage = () => {
  const [limit, setLimit] = useState(20);
  const [page, setPage] = useState(1);
  const [transactionsToShow, setTransactionsToShow] = useState(null);

  const transactions = useLoaderData();
  const totalPages = Math.ceil(transactions.length / limit);

  useEffect(() => {
    handleSetTransactionsToShow();
  }, [limit, page, transactions]);

  const handleChangeLimit = (e) => {
    e.preventDefault();
    setLimit(Number(e.target.value));
    setPage(1);
  };

  const handleChangePage = (e) => {
    e.preventDefault();
    setPage(Number(e.target.value));
  };

  const handleSetTransactionsToShow = () => {
    const start = (page - 1) * limit;
    const end = start + limit;
    setTransactionsToShow(transactions.slice(start, end));
  };

  const handlePreviousPage = () => {
    if (page > 1) {
      setPage(page - 1);
    }
  };

  const handleNextPage = () => {
    if (page < totalPages) {
      setPage(page + 1);
    }
  };

  const pageSelector = (
    <div className="flex flex-row space-x-2 items-center">
      <button
        className={`p-2 border-2 border-black ${
          page === 1 ? "opacity-50" : ""
        }`}
        onClick={handlePreviousPage}
        disabled={page === 1}
      >
        <FaChevronLeft />
      </button>

      <label htmlFor="page-select">Page:</label>
      <select
        className="w-24 border-black border-2 p-1"
        name="page-select"
        id="page-select"
        value={page}
        onChange={handleChangePage}
      >
        {Array.from({ length: totalPages }, (_, i) => (
          <option key={i + 1} value={i + 1}>
            {i + 1}
          </option>
        ))}
      </select>

      <button
        className={`p-2 border-2 border-black ${
          page === totalPages ? "opacity-50" : ""
        }`}
        onClick={handleNextPage}
        disabled={page === totalPages}
      >
        <FaChevronRight />
      </button>
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
        onChange={handleChangeLimit}
      >
        <option value="10">10</option>
        <option value="20">20</option>
        <option value="50">50</option>
      </select>
    </div>
  );

  return (
    <div className="min-h-full">
      <h2 className="pageheading">Transactions</h2>

      {transactions && transactions.length > 0 ? (
        <div className="container mx-auto max-w-6xl flex flex-col space-y-5 py-6">
          <div className="flex flex-row items-center space-x-4">
            {pageSelector}
            {limitSelector}
          </div>
          <TransactionsList transactions={transactionsToShow} />
          <div className="flex flex-row items-center">{pageSelector}</div>
        </div>
      ) : (
        <p>
          There are no transactions to show.{" "}
          <Link to="/home/add-data">Add data</Link>
        </p>
      )}
    </div>
  );
};

const transactionsLoader = async ({ params }) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/transactions`,
      {
        method: "GET",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      throw new Error(response.statusText || "Failed to fetch transactions");
    }

    const data = await response.json();
    return data.transactions;
  } catch (error) {
    console.error("Error fetching transactions:", error);
  }
};

export { TransactionsPage as default, transactionsLoader };
