import dayjs from "dayjs";

export const getDateRangesForPeriod = (periodType, offset = 0) => {
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

export const filterTransactionsByDateRange = (
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
