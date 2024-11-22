import Category from "./Category";
import { formatCurrency } from "../utils/format";

const BudgetLimit = ({ limit }) => {
  const overUnder = limit.amount + limit.actualAmount;

  return (
    <div className="flex flex-row items-center h-16 w-full space-x-3 p-2 bg-pastelYellow text-sm">
      <div className="w-40">
        <Category
          id={limit.categoryId}
          name={limit.categoryName}
          displayName={true}
        />
      </div>
      <div className={"text-left font-bold text-black"}>
        {formatCurrency(limit.amount)}
      </div>
      <div className="grow w-20 text-right">
        <div
          className={
            "font-bold " +
            (limit.actualAmount === 0
              ? "text-gray-400"
              : limit.actualAmount > 0
              ? "text-green-600"
              : "text-red-500")
          }
        >
          {formatCurrency(-limit.actualAmount)}
        </div>
        {limit.actualAmount !== 0 && (
          <div
            className={
              "font-bold " + (overUnder > 0 ? "text-green-600" : "text-red-500")
            }
          >
            ({formatCurrency(overUnder)} {overUnder > 0 ? "under" : "over"})
          </div>
        )}
      </div>
    </div>
  );
};

export default BudgetLimit;
