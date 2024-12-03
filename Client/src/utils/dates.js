import dayjs from "dayjs";
import isSameOrAfter from "dayjs/plugin/isSameOrAfter";
import isSameOrBefore from "dayjs/plugin/isSameOrBefore";

dayjs.extend(isSameOrAfter);
dayjs.extend(isSameOrBefore);

export const getDateRangesForPeriod = (periodType, offset = 0) => {
  switch (periodType) {
    case "monthly":
      return getMonthlyDateRange(offset);
    case "weekly":
      return getWeeklyDateRange(offset);
    case "quarterly":
      return getQuarterlyDateRange(offset);
    case "annual":
      return getAnnualDateRange(offset);
    default:
      console.warn("Unsupported period type:", periodType);
      return { startDates: [], endDates: [] };
  }
};

const getMonthlyDateRange = (offset) => {
  const adjustedDate = dayjs().add(offset, "month");
  const startDates = [adjustedDate.startOf("month")];
  const endDates = [adjustedDate.endOf("month")];
  return { startDates, endDates };
};

const getWeeklyDateRange = (offset) => {
  const adjustedDate = dayjs().add(offset, "week");
  const startDates = [adjustedDate.startOf("week")];
  const endDates = [adjustedDate.endOf("week")];
  return { startDates, endDates };
};

const getQuarterlyDateRange = (offset) => {
  const financialYearStart = dayjs(`${dayjs().year()}-07-01`); // Financial year starts on July 1st
  const quarters = [
    { startMonth: 7, endMonth: 9, endDay: 30 }, // Q1: Jul 1 - Sep 30
    { startMonth: 10, endMonth: 12, endDay: 31 }, // Q2: Oct 1 - Dec 31
    { startMonth: 1, endMonth: 3, endDay: 31 }, // Q3: Jan 1 - Mar 31
    { startMonth: 4, endMonth: 6, endDay: 30 }, // Q4: Apr 1 - Jun 30
  ];

  const baseDate = dayjs().add(offset * 3, "month");
  const currentYear = baseDate.year();
  const currentQuarterIndex =
    Math.floor(baseDate.diff(financialYearStart, "month") / 3) % 4;

  const adjustedQuarterIndex =
    currentQuarterIndex < 0
      ? (currentQuarterIndex + 4) % 4
      : currentQuarterIndex;

  const quarter = quarters[adjustedQuarterIndex];

  const startYear = adjustedQuarterIndex < 2 ? currentYear : currentYear - 1;
  const endYear = adjustedQuarterIndex < 2 ? currentYear : currentYear - 1;

  const startDates = [dayjs(`${startYear}-${quarter.startMonth}-01`)];
  const endDates = [dayjs(`${endYear}-${quarter.endMonth}-${quarter.endDay}`)];

  return { startDates, endDates };
};

const getAnnualDateRange = (offset) => {
  const adjustedDate = dayjs().add(offset, "year");
  const startDates = [adjustedDate.startOf("year")];
  const endDates = [adjustedDate.endOf("year")];
  return { startDates, endDates };
};

// Function to filter transactions by date range
export const filterTransactionsByDateRange = (
  transactions,
  startDates,
  endDates
) => {
  // Input validation
  if (!transactions)
    throw new Error(
      "filterTransactionsByDateRange: transactions is not an array!"
    );

  if (!Array.isArray(startDates) || startDates.length === 0)
    throw new Error("Invalid startDates input: Must be a non-empty array.");

  if (!Array.isArray(endDates) || endDates.length === 0)
    throw new Error("Invalid endDates input: Must be a non-empty array.");

  const startDate = dayjs(startDates[0]);
  const endDate = dayjs(endDates[0]);

  if (!startDate.isValid() || !endDate.isValid()) {
    throw new Error(
      "Invalid dates: Ensure startDates and endDates contain valid ISO date strings."
    );
  }

  // Filter transactions
  return transactions.filter((transaction) => {
    const transactionDate = dayjs(transaction.date);

    if (!transactionDate.isValid()) {
      console.warn(`Skipping invalid transaction date: ${transaction.date}`);
      return false; // Exclude invalid transaction dates
    }

    return (
      transactionDate.isSameOrAfter(startDate) &&
      transactionDate.isSameOrBefore(endDate)
    );
  });
};
