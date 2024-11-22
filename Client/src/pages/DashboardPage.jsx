import { useState } from "react";
import RecentTransactions from "../components/RecentTransactions";
import Summary from "../components/Summary";
import PeriodSelector from "../components/PeriodSelector";
import BudgetLimits from "../components/BudgetLimits";

const DashboardPage = () => {
  const [periodType, setPeriodType] = useState("annual");

  const handlePeriodSelectChange = (e) => {
    e.preventDefault();
    setPeriodType(e.target.value);
  };

  return (
    <div className="flex flex-col space-y-5">
      <h2 className="text-4xl mb-4">Dashboard</h2>

      <div className="flex flex-col md:flex-row content-start md:space-x-8 mb-8">
        <div className="flex flex-col space-y-5 w-full md:w-1/2">
          <PeriodSelector
            periodType={periodType}
            handlePeriodSelectChange={handlePeriodSelectChange}
          />
          {/* SUMMARY */}
          <Summary periodType={periodType} />

          {/* BUDGET SETTINGS */}
          <BudgetLimits periodType={periodType} isOnDashboard={true} />
        </div>

        {/* RECENT TRANSACTIONS */}
        <div className="w-full md:w-1/2">
          <RecentTransactions />
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
