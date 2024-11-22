import React, { useEffect, useState } from "react";

const CurrencyInput = ({ value, onChange, isInvalid }) => {
  const [inputValue, setInputValue] = useState(value || "");

  useEffect(() => {
    if (!value) {
      setInputValue("");
    }
  }, [value]);

  const formatCurrency = (value) => {
    if (!value) return "";

    const cleanedValue = value.replace(/[^0-9.]/g, "");

    const [integerPart, decimalPart] = cleanedValue.split(".");

    const formattedInteger = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    return decimalPart !== undefined
      ? `${formattedInteger}.${decimalPart.slice(0, 2)}`
      : formattedInteger;
  };

  const handleChange = (e) => {
    let value = e.target.value;

    const valueWithoutCommas = value.replace(/,/g, "");
    const validInput = valueWithoutCommas.match(/^[0-9]*\.?[0-9]{0,2}$/);

    if (validInput) {
      const formattedValue = formatCurrency(valueWithoutCommas);
      setInputValue(formattedValue);
      onChange && onChange(formattedValue);
    }
  };

  return (
    <div className="flex flex-row space-x-2 items-center">
      <input
        className={isInvalid ? "invalid" : ""}
        type="text"
        value={inputValue}
        onChange={handleChange}
        placeholder="Enter amount"
        inputMode="decimal"
      />
    </div>
  );
};

export default CurrencyInput;
