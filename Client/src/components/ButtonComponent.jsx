import React from "react";

const ButtonComponent = ({
  children,
  onClick,
  type = "button",
  className = "",
}) => {
  return (
    <button
      onClick={onClick}
      type={type}
      className={`p-2.5 bg-pastelGreen text-black border-2 border-black hover:bg-pastelLightGreen ${className}`}
    >
      {children}
    </button>
  );
};

export default ButtonComponent;
