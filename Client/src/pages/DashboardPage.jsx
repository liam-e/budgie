import { useState } from "react";
import RecentTransactions from "../components/RecentTransactions";
import Summary from "../components/Summary";
import PeriodSelector from "../components/PeriodSelector";
import Chart from "../components/Chart";
import Categorizations from "../components/Categorizations";
import { useLoaderData } from "react-router-dom";
import LinkComponent from "../components/LinkComponent";

const DashboardPage = () => {
  const [periodType, setPeriodType] = useState("monthly");
  const transactions = useLoaderData();

  const handlePeriodSelectChange = (e) => {
    e.preventDefault();
    setPeriodType(e.target.value);
  };

  return (
    <div className="flex flex-col space-y-5">
      <h2 className="pageheading">Dashboard</h2>

      {transactions && transactions.length > 0 ? (
        <div className="flex flex-col md:flex-row content-start md:space-x-8 mb-8 min-w-sm">
          <div className="flex flex-col space-y-5 w-full mb-8 md:w-1/2">
            <PeriodSelector
              periodType={periodType}
              handlePeriodSelectChange={handlePeriodSelectChange}
            />
            {/* SUMMARY */}
            <Summary periodType={periodType} />
          </div>

          <div className="w-full md:w-1/2 mb-8">
            {/* CHART */}
            <Chart periodType={periodType} />

            {/* CATEGORIZATIONS */}
            <Categorizations isOnDashboard={true} />

            {/* RECENT TRANSACTIONS */}
            <RecentTransactions />
          </div>
        </div>
      ) : (
        <>
          <p className="text-gray-500">There is no data to display.</p>
          <LinkComponent to="/home/add-data">
            Add data to get started
          </LinkComponent>
        </>
      )}
    </div>
  );
};

export default DashboardPage;
