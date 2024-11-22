export const sumByCategory = (transactions) => {
  const grouped = transactions.reduce((acc, t) => {
    if (!acc[t.categoryId]) {
      acc[t.categoryId] = t.amount;
    }

    acc[t.categoryId] += t.amount;

    return acc;
  }, {});

  return Object.entries(grouped)
    .map(([categoryId, amount]) => ({
      categoryId,
      amount,
    }))
    .sort((a, b) => b.amount - a.amount);
};
