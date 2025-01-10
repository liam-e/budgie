import { useLocation } from "react-router-dom";
import TransactionForm from "../forms/TransactionForm";

const EditTransactionPage = () => {
  const location = useLocation();

  return (
    <div className="centerboxparent">
      <div className="centerboxchild colorbox flex flex-col  space-y-4 p-12 items-center">
        <h2 className="pageheading">Edit Transaction</h2>
        <TransactionForm
          transaction={location.state?.transaction}
          edit={true}
        />
      </div>
    </div>
  );
};

export default EditTransactionPage;
