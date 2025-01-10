import Category from "./Category";
import { IconContext } from "react-icons";
import {
  formatCurrency,
  formatDescription,
  formatDate,
  formatDateLong,
} from "../utils/format.js";
import EditCategoryForm from "../forms/EditCategoryForm.jsx";
import ButtonComponent from "./ButtonComponent.jsx";
import DeleteButton from "./DeleteButton.jsx";
import { FaEdit, FaTrash } from "react-icons/fa";
import ButtonSpinner from "./ButtonSpinner.jsx";
import { useState } from "react";
import { message } from "./MessageContainer.jsx";
import { Link, Navigate, useNavigate } from "react-router-dom";
import { getUserData } from "../utils/auth.js";

const Transaction = ({
  transaction,
  idx,
  categorize = false,
  onPage = false,
}) => {
  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState(false);

  const sourceId = getUserData()?.integrationKeySource;

  const formattedDescription = formatDescription(transaction.description);

  const formattedAmount = formatCurrency(transaction.amount);
  const formattedDate = formatDate(transaction.date);
  const formattedDateLong = formatDateLong(transaction.date);

  const deleteTransaction = async (transactionId) => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Transactions/${transactionId}`,
        {
          method: "DELETE",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        console.log("Transaction deleted successfully.");
      } else {
        const errorData = await response.json();
        console.error("Error deleting transaction:", errorData);
      }
    } catch (error) {
      console.error("An unexpected error occurred:", error);
    }
  };

  const handleDeleteTransaction = async () => {
    try {
      setIsLoading(true);
      await deleteTransaction(transaction.id);
      message(`Transaction "${formattedDescription}" removed.`);

      setIsLoading(false);
      return navigate(-1);
    } catch (error) {
      console.error("Error deleting transaction:", error);
      setIsLoading(false);
    }
  };

  return onPage ? (
    <div className="centerboxparent">
      <div className="colorbox flex flex-col py-16 px-8 space-y-4 max-w-lg m-auto">
        {/* Transaction Header */}
        <div className="flex flex-row justify-between items-center">
          <h2 className="text-xl font-bold">Transaction Details</h2>
        </div>

        {/* Transaction Information */}
        <div className="flex flex-col space-y-4 text-sm">
          <div className="flex flex-row justify-between">
            <span className="font-medium">Date:</span>

            <span>{formattedDateLong}</span>
          </div>
          <div className="flex flex-row justify-between">
            <span className="font-medium">Description:</span>
            <span>{formattedDescription}</span>
          </div>
          <div className="flex flex-row justify-between">
            <span className="font-medium">Category:</span>
            <div className="flex flex-row items-center space-x-2">
              <Category
                id={transaction.categoryId}
                name={transaction.categoryName}
                displayName={false}
              />
              <span>{transaction.categoryName}</span>
            </div>
          </div>
          <div className="flex flex-row justify-between">
            <span className="font-medium">Amount:</span>
            <span
              className={
                transaction.amount > 0
                  ? "text-green-600 font-bold"
                  : "text-red-500 font-bold"
              }
            >
              {formattedAmount}
            </span>
          </div>
        </div>
        {!sourceId && (
          <div className="flex space-x-2">
            <ButtonComponent
              onClick={() =>
                navigate(`/home/transactions/${transaction.id}/edit`, {
                  state: { transaction },
                })
              }
            >
              <div className="flex flex-row space-x-1 items-center">
                <FaEdit />
                <span>Edit</span>
              </div>
            </ButtonComponent>

            {/* DELETE BUTTON */}
            <DeleteButton onClick={handleDeleteTransaction}>
              {isLoading ? (
                <ButtonSpinner />
              ) : (
                <div className="flex flex-row space-x-1 items-center">
                  <FaTrash />
                  <span>Delete</span>
                </div>
              )}
            </DeleteButton>
          </div>
        )}
      </div>
    </div>
  ) : (
    <div className="flex flex-col">
      <div
        className={`flex flex-row p-2 items-center text-sm ${
          idx % 2 === 0 ? "bg-pastelYellow" : "bg-transparent"
        }`}
      >
        {/* DATE */}
        <div className="w-16 text-nowrap">{formattedDate}</div>

        {/* DESCRIPTION */}
        <div
          className={`flex-grow flex flex-row space-x-5 items-center ${
            categorize ? "hover:no-underline" : "hover:underline"
          }`}
        >
          {/* CATEGORY */}
          {categorize ? (
            <div className="flex grow-0">
              <EditCategoryForm transaction={transaction} />
            </div>
          ) : (
            <IconContext.Provider
              value={{ color: "black", className: "global-class-name" }}
            >
              <div title={transaction.categoryName}>
                <Category
                  id={transaction.categoryId}
                  name={transaction.categoryName}
                />
              </div>
            </IconContext.Provider>
          )}
          <p className="w-16 flex-grow font-normal text-ellipsis overflow-hidden text-nowrap">
            {formattedDescription}
          </p>
        </div>

        {/* AMOUNT */}
        <div
          className={
            transaction.amount > 0
              ? "text-green-600 font-bold"
              : "text-red-500 font-bold"
          }
        >
          {formattedAmount}
        </div>
      </div>
    </div>
  );
};

export default Transaction;
