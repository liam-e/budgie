import React, { useEffect, useState } from "react";
import { useLoaderData } from "react-router-dom";
import { formatCurrency } from "../utils/format";
import dayjs from "dayjs";

const Summary = () => {
  const transactions = useLoaderData();

  const [title, setTitle] = useState("");
  const [periodType, setPeriodType] = useState("monthly");
  const [income, setIncome] = useState(0);
  const [expenses, setExpenses] = useState(0);
  const [percentChange, setPercentChange] = useState(0);

  useEffect(() => {
    setTitle(generateSummaryTitle(periodType));

    const { startDates, endDates } = getDateRangesForPeriod(periodType);
    const filteredTransactions = filterTransactionsByDateRange(
      transactions,
      startDates,
      endDates
    );

    setIncome(calculateIncome(filteredTransactions));
    setExpenses(calculateExpenses(filteredTransactions));
    setPercentChange(calculatePercentChange(filteredTransactions));
  }, [periodType]);

  const handlePeriodSelectChange = (e) => {
    e.preventDefault();
    setPeriodType(e.target.value);
  };

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

  const getDateRangesForPeriod = (periodType, offset = 0) => {
    const now = dayjs().add(offset, "month");
    let startDates = [];
    let endDates = [];

    switch (periodType) {
      case "monthly":
        startDates.push(now.startOf("month"));
        endDates.push(now.endOf("month").add(1, "day"));
        break;
      case "weekly":
        startDates.push(now.startOf("week"));
        endDates.push(now.endOf("week").add(1, "day"));
        break;
      case "quarterly":
        const currentQuarterStart = now.startOf("quarter");
        startDates.push(currentQuarterStart);
        endDates.push(currentQuarterStart.add(3, "months"));
        break;
      case "annual":
        startDates.push(now.startOf("year"));
        endDates.push(now.endOf("year").add(1, "day"));
        break;
      default:
        // TODO: Custom
        break;
    }

    return { startDates, endDates };
  };

  const filterTransactionsByDateRange = (
    transactions,
    startDates,
    endDates
  ) => {
    return transactions.filter((t) => {
      const transactionDate = dayjs(t.date);
      return (
        transactionDate.isAfter(startDates[0]) &&
        transactionDate.isBefore(endDates[0])
      );
    });
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

  const fieldClasses = "flex flex-row items-center space-x-5 p-2";
  const fieldLabelClasses = "w-24";
  const fieldValueClasses = "font-bold grow text-right";

  return (
    <div className="flex flex-col space-y-5">
      <h3>Summary</h3>

      <select
        className="w-24 border-black"
        name="period-select"
        id="period-select"
        value={periodType}
        onChange={handlePeriodSelectChange}
      >
        <option value="monthly">Monthly</option>
        <option value="weekly">Weekly</option>
        <option value="quarterly">Quarterly</option>
        <option value="annual">Annual</option>
        {/* <option value="custom">Custom..</option> */}
      </select>

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
    </div>
  );
};

export default Summary;
