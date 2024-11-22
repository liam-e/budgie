import React, { useEffect, useState, useRef } from "react";
import Category from "./Category";
import { useRouteLoaderData } from "react-router-dom";

const CategorySelector = ({
  onChange,
  onBlur,
  value,
  name,
  inputRef,
  isInvalid,
}) => {
  const { categories } = useRouteLoaderData("home");
  const [isOpen, setIsOpen] = useState(false);

  const dropdownRef = useRef(null);

  const handleSelectCategory = (category) => {
    value = category;
    setIsOpen(false);
    onChange &&
      onChange({
        id: category.id,
        name: category.name,
        transactionTypeId: category.transactionTypeId,
      });
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setIsOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div
      className={`relative w-60 border-2 ${
        isInvalid
          ? "border-red-600 hover:border-red-500 bg-red-100"
          : "border-black"
      }`}
      ref={dropdownRef}
    >
      {/* Display selected category */}
      <div
        className="p-2.5 cursor-pointer"
        onClick={() => setIsOpen(!isOpen)}
        onChange={onChange}
      >
        {value ? (
          <div className="flex items-center">
            <Category id={value.id} />
            <span className="ml-2">{value.name}</span>
          </div>
        ) : (
          <span className="text-gray-400">Select a Category</span>
        )}
      </div>

      {isOpen && (
        <div className="absolute top-full left-0 right-0 z-10 max-h-52 overflow-y-auto border-2 border-black bg-white">
          {categories.map((category) => (
            <div
              key={category.id}
              className="flex items-center cursor-pointer p-2.5 hover:bg-pastelYellow"
              onClick={() => handleSelectCategory(category)}
            >
              <Category
                id={category.id}
                name={category.name}
                transactionTypeId={category.transactionTypeId}
                displayName={true}
              />
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default CategorySelector;
