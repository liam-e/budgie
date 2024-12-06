import { useState } from "react";
import RecentTransactions from "../components/RecentTransactions";
import Summary from "../components/Summary";
import PeriodSelector from "../components/PeriodSelector";
import BudgetLimits from "../components/BudgetLimits";
import Chart from "../components/Chart";
import Categorizations from "../components/Categorizations";

const DashboardPage = () => {
  const [periodType, setPeriodType] = useState("monthly");

  const handlePeriodSelectChange = (e) => {
    e.preventDefault();
    setPeriodType(e.target.value);
  };

  return (
    <div className="flex flex-col space-y-5">
      <h2 className="pageheading">Dashboard</h2>

      <div className="flex flex-col md:flex-row content-start md:space-x-8 mb-8 min-w-sm">
        <div className="flex flex-col space-y-5 w-full mb-8 md:w-1/2">
          <PeriodSelector
            periodType={periodType}
            handlePeriodSelectChange={handlePeriodSelectChange}
          />
          {/* SUMMARY */}
          <Summary periodType={periodType} />

          {/* BUDGET SETTINGS */}
          <BudgetLimits periodType={periodType} isOnDashboard={true} />
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
    </div>
  );
};

export default DashboardPage;
