import React from "react";
import { useState } from "react";
import Category from "./Category";
import { IconContext } from "react-icons";
import {
  formatCurrency,
  formatDescription,
  formatDate,
} from "../utils/format.js";

const Transaction = ({
  data: {
    date,
    originalDescription,
    modifiedDescription,
    amount,
    categoryName,
  },
  idx,
}) => {
  const [showFullInfo, setShowFullInfo] = useState(false);

  const formattedDescription = modifiedDescription
    ? modifiedDescription
    : formatDescription(originalDescription);

  const formattedAmount = formatCurrency(amount);
  const formattedDate = formatDate(date);

  return (
    <a
      onClick={() => setShowFullInfo((prevState) => !prevState)}
      className="no-underline text-black hover:text-black"
    >
      <div className="flex flex-col">
        <div
          className={`flex flex-row p-2 space-x-3 items-center text-md ${
            idx % 2 === 0 ? "bg-green-200" : "bg-transparent"
          }`}
        >
          {/* DATE */}
          <div className="w-16">{formattedDate}</div>

          {/* CATEGORY */}
          <IconContext.Provider
            value={{ color: "black", className: "global-class-name" }}
          >
            <div title={categoryName}>
              <Category categoryName={categoryName} />
            </div>
          </IconContext.Provider>

          {/* DESCRIPTION */}
          <div className="flex-grow font-normal lowercase">
            {formattedDescription}
          </div>

          {/* AMOUNT */}
          <div
            className={
              amount > 0 ? "text-green-600 font-bold" : "text-red-500 font-bold"
            }
          >
            {formattedAmount}
          </div>
        </div>
        {showFullInfo ? (
          <div
            className={`flex flex-row p-2 space-x-3 items-center text-md ${
              idx % 2 === 0 ? "bg-green-200" : "bg-transparent"
            }`}
          >
            [Full info]
          </div>
        ) : (
          <></>
        )}
      </div>
    </a>
  );
};

export default Transaction;
