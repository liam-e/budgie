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
    categoryId,
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
          className={`flex flex-row p-2 space-x-3 items-center text-sm ${
            idx % 2 === 0 ? "bg-pastelYellow" : "bg-transparent"
          }`}
        >
          {/* DATE */}
          <div className="w-16 text-nowrap">{formattedDate}</div>

          {/* DESCRIPTION */}
          <div className="flex-grow flex flex-row space-x-5 items-center">
            {/* CATEGORY */}
            <IconContext.Provider
              value={{ color: "black", className: "global-class-name" }}
            >
              <div title={categoryName}>
                <Category id={categoryId} name={categoryName} />
              </div>
            </IconContext.Provider>
            <p className="w-16 flex-grow font-normal lowercase text-ellipsis overflow-hidden text-nowrap">
              {formattedDescription}
            </p>
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
            className={`flex flex-row p-2 space-x-3 items-center text-sm ${
              idx % 2 === 0 ? "bg-pastelYellow" : "bg-transparent"
            }`}
          >
            [Full info]
          </div> // TODO: implement full info toggle for transactions
        ) : (
          <></>
        )}
      </div>
    </a>
  );
};

export default Transaction;
