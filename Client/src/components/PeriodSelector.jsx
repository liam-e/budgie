import React from "react";

const PeriodSelector = ({ periodType, handlePeriodSelectChange }) => {
  return (
    <div className="flex flex-row items-center space-x-2">
      <label htmlFor="period-select">Select period:</label>
      <select
        className="w-36 border-black border-2 p-2"
        name="period-select"
        id="period-select"
        value={periodType}
        onChange={handlePeriodSelectChange}
      >
        <option value="monthly">Monthly</option>
        <option value="weekly">Weekly</option>
        <option value="quarterly">Quarterly</option>
        <option value="annual">Annual</option>
        {/* <option value="custom">Custom..</option> */}
      </select>
    </div>
  );
};

export default PeriodSelector;
