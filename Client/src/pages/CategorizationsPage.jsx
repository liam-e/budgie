import Categorizations from "../components/Categorizations";
import { useLoaderData } from "react-router-dom";

const CategorizationsPage = () => {
  const transactions = useLoaderData();
  return (
    <div className="min-h-full">
      <h2 className="pageheading">Categorise transactions</h2>

      <div className="container mx-auto max-w-4xl flex flex-col space-y-5 py-6">
        <Categorizations transactions={transactions} />
      </div>
    </div>
  );
};

export default CategorizationsPage;
