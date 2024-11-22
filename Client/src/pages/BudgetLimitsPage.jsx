import { useState } from "react";
import BudgetLimits from "../components/BudgetLimits";
import PeriodSelector from "../components/PeriodSelector";

const BudgetLimitsPage = () => {
  const [periodType, setPeriodType] = useState("annual");

  const handlePeriodSelectChange = (e) => {
    e.preventDefault();
    setPeriodType(e.target.value);
  };

  return (
    <div className="flex flex-col space-y-8 p-6 items-center">
      <h2 className="text-4xl mb-8">Set Budget Limits</h2>
      <PeriodSelector
        periodType={periodType}
        handlePeriodSelectChange={handlePeriodSelectChange}
      />
      <BudgetLimits
        periodType={periodType}
        setPeriodType={setPeriodType}
        isShowingForm={true}
      />
    </div>
  );
};

const budgetLimitsLoader = async ({ params }) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/BudgetLimits`,
      {
        method: "GET",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      throw new Error(response.statusText || "Failed to fetch budget limits");
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching budget limits:", error);
  }
};

export { BudgetLimitsPage as default, budgetLimitsLoader };
