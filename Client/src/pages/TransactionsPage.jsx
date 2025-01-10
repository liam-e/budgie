import { useEffect, useState } from "react";
import { useLoaderData } from "react-router-dom";
import TransactionsList from "../components/TransactionsList";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import LinkComponent from "../components/LinkComponent";
import {
  getIntegrationKey,
  mapUpCategoryToCategory,
  mapUpTransactionToTransaction,
  sources,
} from "../utils/feeds";

import dayjs from "dayjs";
import timezone from "dayjs/plugin/timezone";
import utc from "dayjs/plugin/utc";
import { getUserData } from "../utils/auth";

dayjs.extend(utc);
dayjs.extend(timezone);

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
      <div className="flex flex-col space-y-5">
        <h2 className="pageheading">Transactions</h2>

        {transactions && transactions.length > 0 ? (
          <div className="container mx-auto max-w-6xl flex flex-col space-y-5 py-2">
            <div className="flex flex-row items-center space-x-4">
              {pageSelector}
              {limitSelector}
            </div>
            <TransactionsList transactions={transactionsToShow} />
          </div>
        ) : (
          <>
            <p>There are no transactions to show. </p>

            <LinkComponent to="/home/add-data">Add data</LinkComponent>
          </>
        )}
      </div>
    </div>
  );
};

const getTransactionsFromFeed = async (sourceId, integrationKey) => {
  try {
    const source = sources.find((s) => s.id === sourceId);
    if (!source) throw new Error(`Source with ID ${sourceId} not found`);

    const startDate = dayjs().subtract(2, "months").startOf("month");

    let allTransactions = [];
    let url = `${source.url}/accounts/${integrationKey.accountId}/transactions`;
    let params = new URLSearchParams({
      "page[size]": 100,
      "filter[since]": startDate.format("YYYY-MM-DDTHH:mm:ssZ"),
    });

    let nextUrl = `${url}?${params.toString()}`;

    while (nextUrl) {
      const response = await fetch(nextUrl, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${integrationKey.key}`,
        },
      });

      if (!response.ok) {
        throw new Error(
          `Failed to fetch transactions from feed: ${response.status} ${response.statusText}`
        );
      }

      const data = await response.json();
      allTransactions = [...allTransactions, ...data.data];

      nextUrl = data.links?.next || null;
    }

    const userId = getUserData()?.id;

    return allTransactions.map((t, idx) =>
      mapUpTransactionToTransaction(t, idx, userId)
    );
  } catch (error) {
    console.error("Error fetching transactions from feed:", error);
    throw error;
  }
};

const getTransactions = async () => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/Transactions`,
      {
        method: "GET",
        credentials: "include",
      }
    );

    if (!response.ok) {
      throw new Error(
        `Failed to fetch transactions: ${response.status} ${response.statusText}`
      );
    }

    const data = await response.json();

    return data.transactions;
  } catch (error) {
    console.error("Error fetching transactions:", error);
    throw error;
  }
};

const transactionsLoader = async () => {
  try {
    const sourceId = getUserData()?.integrationKeySource;

    if (sourceId) {
      const integrationKey = await getIntegrationKey(sourceId);
      if (integrationKey) {
        return await getTransactionsFromFeed(sourceId, integrationKey);
      } else {
        throw Error("Error retrieving IntegrationKey.");
      }
    } else {
      return await getTransactions();
    }
  } catch (error) {
    console.error("Error loading transactions:", error);
    return [];
  }
};

export { TransactionsPage as default, transactionsLoader };
