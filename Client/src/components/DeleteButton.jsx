import React from "react";

const DeleteButton = ({ children, onClick, type = "button" }) => {
  return (
    <button
      className="p-2.5 bg-red-400 text-black border-2 border-black hover:bg-red-300"
      type={type}
      onClick={onClick}
    >
      {children}
    </button>
  );
};

export default DeleteButton;
