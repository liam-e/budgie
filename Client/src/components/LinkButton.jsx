import React from "react";
import { Link } from "react-router-dom";

const LinkButton = ({ to, children }) => {
  return (
    <Link
      to={to}
      className="bg-pastelGreen text-black border-2 border-black p-3 no-underline hover:text-black hover:bg-pastelLightGreen"
    >
      {children}
    </Link>
  );
};

export default LinkButton;
