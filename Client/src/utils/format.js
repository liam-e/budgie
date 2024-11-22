const formatCurrency = (amount) => {
  const isNegative = amount < 0;

  amount = Math.abs(amount);
  const dollars = Math.floor(amount).toLocaleString().split(".")[0];
  let cents =
    Math.round((amount - Math.floor(amount) + Number.EPSILON) * 100) / 100;

  cents = cents === 0 ? "00" : cents.toFixed(2).split(".")[1];

  return isNegative ? `-\$${dollars}.${cents}` : `\$${dollars}.${cents}`;
};

const formatDescription = (desc) => {
  desc = desc.trim();
  if (desc.substring(desc.length - 1) === ";") {
    desc = desc.substring(0, desc.length - 1).trim();
  }
  return desc;
};

const formatDate = (date) => {
  return new Date(date)
    .toLocaleDateString("en-US", {
      month: "short",
      day: "2-digit",
    })
    .toLowerCase();
};

export { formatCurrency, formatDescription, formatDate };
