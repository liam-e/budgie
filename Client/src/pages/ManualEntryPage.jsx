import ManualEntryForm from "../forms/ManualEntryForm";

const ManualEntryPage = () => {
  return (
    <div className="flex flex-col space-y-8 p-6 items-center">
      <h2 className="text-4xl mb-8">Add Transaction</h2>

      <ManualEntryForm />
    </div>
  );
};

export default ManualEntryPage;
