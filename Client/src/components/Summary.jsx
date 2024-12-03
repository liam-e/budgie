import React, { useMemo } from "react";
import { useLoaderData } from "react-router-dom";
import { formatCurrency } from "../utils/format";
import {
  getDateRangesForPeriod,
  filterTransactionsByDateRange,
} from "../utils/dates";
import dayjs from "dayjs";
import TopExpenses from "./TopExpenses";

const Summary = ({ periodType }) => {
  const transactions = useLoaderData();

  const { title, filteredTransactions, income, expenses, percentChange } =
    useMemo(() => {
      const now = dayjs();
      const { startDates, endDates } = getDateRangesForPeriod(periodType);
      const filtered = filterTransactionsByDateRange(
        transactions,
        startDates,
        endDates
      );

      const income = filtered
        .map((t) => t.amount)
        .filter((amount) => amount > 0)
        .reduce((a, b) => a + b, 0);

      const expenses = -filtered
        .map((t) => t.amount)
        .filter((amount) => amount < 0)
        .reduce((a, b) => a + b, 0);

      const { startDates: prevStart, endDates: prevEnd } =
        getDateRangesForPeriod(periodType, -1);
      const prevTransactions = filterTransactionsByDateRange(
        transactions,
        prevStart,
        prevEnd
      );

      const prevIncome = prevTransactions
        .map((t) => t.amount)
        .filter((amount) => amount > 0)
        .reduce((a, b) => a + b, 0);

      const percentChange =
        prevIncome === 0
          ? "N/A"
          : `${(((income - prevIncome) / prevIncome) * 100).toFixed(2)}%`;

      const title =
        {
          monthly: now.format("MMMM YYYY"),
          weekly: `${now.startOf("week").format("MMM D")} - ${now
            .endOf("week")
            .format("MMM D, YYYY")}`,
          quarterly: `Q${Math.ceil((now.month() + 1) / 3)} ${now.format(
            "YYYY"
          )}`,
          annual: now.format("YYYY"),
        }[periodType] || "Custom Period";

      return {
        title,
        filteredTransactions: filtered,
        income: formatCurrency(income),
        expenses: formatCurrency(expenses),
        percentChange,
      };
    }, [transactions, periodType]);

  const fieldClasses =
    "flex flex-row items-center space-x-5 px-3 py-4 text-sm min-w-fit";
  const fieldValueClasses = "font-bold grow text-right";

  return filteredTransactions && filteredTransactions.length > 0 ? (
    <div className="flex flex-col space-y-5">
      <h3>Summary</h3>
      <h4>{title}</h4>

      <div className={`${fieldClasses} bg-green-200`}>
        <h4>Income:</h4>
        <div className={`${fieldValueClasses} text-green-600`}>{income}</div>
      </div>
      <div className={`${fieldClasses} bg-red-200`}>
        <h4>Expenses:</h4>
        <div className={`${fieldValueClasses} text-red-500`}>{expenses}</div>
      </div>
      <div
        className={`${fieldClasses} ${
          percentChange[0] !== "-" ? "bg-green-200" : "bg-red-200"
        }`}
      >
        <h4>% Change:</h4>
        <div
          className={`${fieldValueClasses} ${
            percentChange[0] !== "-" ? "text-green-600" : "text-red-500"
          }`}
        >
          {percentChange}
        </div>
      </div>

      <TopExpenses transactions={filteredTransactions} />
    </div>
  ) : (
    <p>There are no transactions for this period.</p>
  );
};

export default Summary;
