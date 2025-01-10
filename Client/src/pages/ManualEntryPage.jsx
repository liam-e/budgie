import TransactionForm from "../forms/TransactionForm";

const ManualEntryPage = () => {
  return (
    <div className="centerboxparent">
      <div className="centerboxchild colorbox">
        <div className="flex flex-col items-center px-4 py-8">
          <h2 className="pageheading">Add Transaction</h2>

          <TransactionForm />
        </div>
      </div>
    </div>
  );
};

export default ManualEntryPage;
