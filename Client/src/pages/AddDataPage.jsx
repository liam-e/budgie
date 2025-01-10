import { FaDatabase, FaUpload } from "react-icons/fa";
import { FaPenToSquare } from "react-icons/fa6";
import { useLocation, useNavigate } from "react-router-dom";

const AddDataPage = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const queryString = location.search;
  const params = new URLSearchParams(queryString);

  const newUser = params.get("newuser");

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
    // <div className="fullpage">
    <div className="centerboxparent">
      <div className="centerboxchild">
        <div className="flex flex-col">
          <h2 className="pageheading text-center">Add data</h2>
          <p className="text-lg text-center mb-4">
            {newUser
              ? "Choose how you would like to add your transactions to get started."
              : "Choose how you would like to add your transactions."}
          </p>
          <div className="grid grid-cols-1 gap-8 md:grid-cols-3 md:min-w-96">
            {options.map((option, idx) => (
              <div
                key={idx}
                onClick={option.handleClick}
                className="adddatacard"
              >
                <div className="p-3">{option.icon}</div>
                <h3 className="text-lg font-semibold">{option.heading}</h3>
                <p className="text-sm mt-2">{option.text}</p>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
    // </div>
  );
};

export default AddDataPage;
