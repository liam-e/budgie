import React, { useEffect, useState } from "react";
import { useLoaderData } from "react-router-dom";
import { formatCurrency } from "../utils/format";
import {
  getDateRangesForPeriod,
  filterTransactionsByDateRange,
} from "../utils/dates";
import { sumByCategory } from "../utils/calc";
import dayjs from "dayjs";
import TopExpenses from "./TopExpenses";

const Summary = ({ periodType }) => {
  const transactions = useLoaderData();

  const [title, setTitle] = useState("");
  const [income, setIncome] = useState(0);
  const [expenses, setExpenses] = useState(0);
  const [percentChange, setPercentChange] = useState(0);
  const [categoryAmounts, setCategoryAmounts] = useState([]);
  const [filteredTransactions, setFilteredTransactions] = useState([]);

  useEffect(() => {
    setTitle(generateSummaryTitle(periodType));

    const { startDates, endDates } = getDateRangesForPeriod(periodType);

    setFilteredTransactions(
      filterTransactionsByDateRange(transactions, startDates, endDates)
    );

    setIncome(calculateIncome(filteredTransactions));
    setExpenses(calculateExpenses(filteredTransactions));
    setPercentChange(calculatePercentChange(filteredTransactions));

    setCategoryAmounts(sumByCategory(filteredTransactions));
  }, [periodType]);

  const generateSummaryTitle = (periodType) => {
    const now = dayjs();

    switch (periodType) {
      case "monthly":
        return now.format("MMMM YYYY");
      case "weekly":
        const startOfWeek = now.startOf("week").format("MMM D, YYYY");
        const endOfWeek = now.endOf("week").format("MMM D, YYYY");
        return `${startOfWeek} - ${endOfWeek}`;
      case "quarterly":
        const currentQuarter = Math.ceil((now.month() + 1) / 3);
        return `Q${currentQuarter} ${now.format("YYYY")}`;
      case "annual":
        return now.format("YYYY");
      default:
        return "Custom Period";
    }
  };

  const calculateIncome = (filteredTransactions) => {
    return formatCurrency(
      filteredTransactions
        .map((t) => t.amount)
        .filter((amount) => amount > 0)
        .reduce((a, b) => a + b, 0)
    );
  };

  const calculateExpenses = (filteredTransactions) => {
    return formatCurrency(
      -filteredTransactions
        .map((t) => t.amount)
        .filter((amount) => amount < 0)
        .reduce((a, b) => a + b, 0)
    );
  };

  const calculatePercentChange = (filteredTransactions) => {
    const { startDates, endDates } = getDateRangesForPeriod(periodType, -1);
    const previousTransactions = filterTransactionsByDateRange(
      transactions,
      startDates,
      endDates
    );

    const currentIncome = filteredTransactions
      .map((t) => t.amount)
      .filter((amount) => amount > 0)
      .reduce((a, b) => a + b, 0);

    const previousIncome = previousTransactions
      .map((t) => t.amount)
      .filter((amount) => amount > 0)
      .reduce((a, b) => a + b, 0);

    if (previousIncome === 0) {
      return "N/A"; // Prevent division by 0
    }

    const percentChange =
      ((currentIncome - previousIncome) / previousIncome) * 100;
    return `${percentChange.toFixed(2)}%`;
  };

  const fieldClasses = "flex flex-row items-center space-x-5 p-2 text-sm";
  const fieldLabelClasses = "";
  const fieldValueClasses = "font-bold grow text-right";

  return filteredTransactions && filteredTransactions.length > 0 ? (
    <div className="flex flex-col space-y-5">
      <h3>Summary</h3>

      <h4>{title}</h4>

      <div className={fieldClasses + " bg-green-200"}>
        <h4 className={fieldLabelClasses}>Income:</h4>
        <div className={fieldValueClasses + " text-green-600"}>{income}</div>
      </div>
      <div className={fieldClasses + " bg-red-200"}>
        <h4 className={fieldLabelClasses}>Expenses:</h4>
        <div className={fieldValueClasses + " text-red-500"}>{expenses}</div>
      </div>
      <div
        className={
          percentChange[0] !== "-"
            ? fieldClasses + " bg-green-200"
            : fieldClasses + " bg-red-200"
        }
      >
        <h4 className={fieldLabelClasses}>% Change:</h4>
        <div
          className={
            percentChange[0] !== "-"
              ? fieldValueClasses + " text-green-600"
              : fieldValueClasses + " text-red-500"
          }
        >
          {percentChange}
        </div>
      </div>

      <TopExpenses categoryAmounts={categoryAmounts} />
    </div>
  ) : (
    <p>There are no transactions for this period.</p>
  );
};

export default Summary;
