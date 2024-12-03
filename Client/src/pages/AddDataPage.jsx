import React from "react";
import { FaDatabase, FaUpload } from "react-icons/fa";
import { FaPenToSquare } from "react-icons/fa6";
import { useNavigate } from "react-router-dom";

const AddDataPage = () => {
  const navigate = useNavigate();

  const options = [
    {
      heading: "Upload CSV File",
      text: "Import transactions from your bank",
      icon: <FaUpload size="20px" />,
      handleClick: () => navigate("/home/upload-csv"),
    },
    {
      heading: "Add Data Feed",
      text: "Connect to a data feed for real-time data",
      icon: <FaDatabase size="20px" />,
      handleClick: () => navigate("/home/add-feed"),
    },
    {
      heading: "Enter Transactions Manually",
      text: "Add your transactions manually",
      icon: <FaPenToSquare size="20px" />,
      handleClick: () => navigate("/home/manual-entry"),
    },
  ];

  return (
    <div className="flex flex-col items-center justify-center min-h-full space-y-8 py-10">
      <h2 className="text-4xl mb-4">Add data</h2>
      <p className="text-lg text-center mb-4">
        Choose how you would like to add your transactions to get started.
      </p>
      <div className="grid grid-cols-1 gap-8 md:grid-cols-3 min-w-10">
        {options.map((option, idx) => (
          <div
            key={idx}
            onClick={option.handleClick}
            className="cursor-pointer transition-transform flex flex-col items-center justify-center px-6 py-8 border-4 border-pastelGreen bg-pastelYellow hover:bg-pastelLightYellow text-center min-w-full"
          >
            <div className="p-3">{option.icon}</div>
            <h3 className="text-lg font-semibold">{option.heading}</h3>
            <p className="text-sm mt-2">{option.text}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default AddDataPage;
