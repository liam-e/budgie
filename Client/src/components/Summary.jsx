import React, { useMemo, useState } from "react";
import { useLoaderData } from "react-router-dom";
import { formatCurrency } from "../utils/format";
import {
  getDateRangesForPeriod,
  filterTransactionsByDateRange,
} from "../utils/dates";
import dayjs from "dayjs";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import TopExpenses from "./TopExpenses";
import BudgetLimits from "./BudgetLimits";

const Summary = ({ periodType }) => {
  const minOffset = -12;
  const maxOffset = 0;
  const transactions = useLoaderData();
  const [offset, setOffset] = useState(0);

  const { title, filteredTransactions, income, expenses, percentChange } =
    useMemo(() => {
      const now = dayjs().add(
        offset,
        periodType === "monthly"
          ? "month"
          : periodType === "weekly"
          ? "week"
          : periodType === "quarterly"
          ? "quarter"
          : periodType === "annual"
          ? "year"
          : "day"
      );

      const { startDates, endDates } = getDateRangesForPeriod(
        periodType,
        offset
      );
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
        getDateRangesForPeriod(periodType, offset - 1);
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
    }, [transactions, periodType, offset]);

  const offsetWithinRange = (newOffset) => {
    return newOffset <= maxOffset && newOffset >= minOffset;
  };

  const handleChangeOffset = (delta) => {
    if (offsetWithinRange(offset + delta)) {
      setOffset(offset + delta);
    }
  };

  const InfoBlock = ({ label, value, positive = true }) => (
    <div
      className={`flex flex-row items-center space-x-5 px-3 py-5 text-sm min-w-fit ${
        positive ? "bg-green-200" : "bg-red-200"
      }`}
    >
      <h4>{label}:</h4>
      <div
        className={`font-bold grow text-right ${
          positive ? "text-green-600" : "text-red-500"
        }`}
      >
        {value}
      </div>
    </div>
  );

  return (
    <div className="flex flex-col space-y-5">
      <div className="flex items-center space-x-2">
        <button
          className={`p-2 border-2 border-black ${
            offsetWithinRange(offset - 1) ? "" : "opacity-50"
          }`}
          onClick={() => handleChangeOffset(-1)}
        >
          <FaChevronLeft />
        </button>
        <h4 className="flex-grow text-center">
          {title + (offset === 0 ? " (current)" : "")}
        </h4>
        <button
          className={`p-2 border-2 border-black ${
            offsetWithinRange(offset + 1) ? "" : "opacity-50"
          }`}
          onClick={() => handleChangeOffset(1)}
        >
          <FaChevronRight />
        </button>
      </div>

      {filteredTransactions && filteredTransactions.length > 0 ? (
        <>
          <InfoBlock label="Income" value={income} positive={true} />
          <InfoBlock label="Expenses" value={expenses} positive={false} />
          <InfoBlock
            label="% Change"
            value={percentChange}
            positive={percentChange[0] !== "-"}
          />

          <TopExpenses transactions={filteredTransactions} />

          {/* BUDGET SETTINGS */}
          <BudgetLimits
            periodType={periodType}
            offset={offset}
            isOnDashboard={true}
          />
        </>
      ) : (
        <p>There are no transactions for this period.</p>
      )}
    </div>
  );
};

export default Summary;
