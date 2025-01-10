import dayjs from "dayjs";

export const sumByCategory = (transactions) => {
  if (!transactions)
    throw new Error("sumByCategory: transactions is not an array!");

  if (transactions.length === 0)
    console.warn("sumByCategory: transactions is empty.");

  const grouped = transactions.reduce((acc, t) => {
    if (!acc[t.categoryId]) {
      acc[t.categoryId] = 0; // Initialize to 0
    }

    acc[t.categoryId] += t.amount; // Add t.amount
    return acc;
  }, {});

  const d = Object.entries(grouped).map(([categoryId, amount]) => ({
    categoryId,
    amount,
  }));

  return d.sort((a, b) => b.amount - a.amount);
};

export const sumTransactionsByPeriod = (transactions, periodType) => {
  const groupedData = {};

  transactions.forEach((transaction) => {
    const transactionDate = dayjs(transaction.date);
    let periodKey;

    switch (periodType) {
      case "annual":
        periodKey = transactionDate.format("YYYY"); // Group by year
        break;
      case "monthly":
        periodKey = transactionDate.format("YYYY-MM"); // Group by month
        break;
      case "weekly":
        periodKey = transactionDate.startOf("week").format("YYYY-MM-DD"); // Group by week start date
        break;
      case "quarterly":
        const quarter = Math.ceil((transactionDate.month() + 1) / 3); // Calculate the quarter
        periodKey = `${transactionDate.format("YYYY")}-Q${quarter}`; // Group by year and quarter
        break;
      default:
        throw new Error("Unsupported period type");
    }

    if (!groupedData[periodKey]) {
      groupedData[periodKey] = {
        name: generatePeriodName(transactionDate, periodType),
        Income: 0,
        Expenses: 0,
      };
    }

    if (transaction.amount > 0) {
      groupedData[periodKey].Income += transaction.amount;
    } else {
      groupedData[periodKey].Expenses += Math.abs(transaction.amount); // Convert to positive for expenses
    }
  });

  // Convert grouped data back to an array and sort by periodKey
  return Object.entries(groupedData)
    .sort(([keyA], [keyB]) => keyA.localeCompare(keyB)) // Sort by the period key
    .map(([_, value]) => value); // Return the values (sorted)
};

const generatePeriodName = (startDate, periodType) => {
  const date = dayjs(startDate);

  switch (periodType) {
    case "monthly":
      return date.format("MMMM");
    case "weekly":
      const startOfWeek = date.startOf("week").format("MMM D");
      const endOfWeek = date.endOf("week").format("MMM D");
      return `${startOfWeek} - ${endOfWeek}`;
    case "quarterly":
      const quarter = Math.ceil((date.month() + 1) / 3);
      return `Q${quarter} ${date.format("YYYY")}`;
    case "annual":
      return date.format("YYYY");
    default:
      return "Custom";
  }
};
