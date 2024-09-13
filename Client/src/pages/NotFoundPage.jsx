import React from "react";
import { Link } from "react-router-dom";
import ButtonComponent from "../components/ButtonComponent";

const NotFoundPage = () => {
  return (
    <div className="flex flex-col items-center justify-center h-screen">
      <div className="text-center p-6">
        <h1 className="text-4xl font-bold text-pastelGreen mb-4">
          Oops! Page not found
        </h1>
        <p className="text-lg text-pastelDarkGreen mb-6">
          It looks like the page you're trying to visit doesn't exist.
        </p>
        <Link to="/">
          <ButtonComponent>Take me back home</ButtonComponent>
        </Link>
      </div>
    </div>
  );
};

export default NotFoundPage;
