import Transaction from "../components/Transaction";
import { useLocation, useRouteLoaderData } from "react-router-dom";

const TransactionPage = () => {
  const { mapCategoryIdToName } = useRouteLoaderData("home");
  const location = useLocation();

  const transaction = location.state?.transaction;

  if (!transaction) {
    return <p>Transaction not found.</p>;
  }

  return (
    <Transaction
      transaction={{
        ...transaction,
        categoryName: mapCategoryIdToName[transaction.categoryId],
      }}
      onPage={true}
    />
  );
};

export default TransactionPage;
