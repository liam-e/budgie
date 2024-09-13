import React from "react";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import ButtonComponent from "./ButtonComponent"; // Import reusable ButtonComponent

const LogoutButton = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    return logout()
      .then(() => {
        console.log("Successfully logged out!");
        navigate("/login");
      })
      .catch((error) => {
        console.error("Logout failed", error);
      });
  };

  const linkClass =
    "px-3 py-3 rounded-md text-sm font-medium text-pastelDarkGreen hover:text-pastelGreen no-underline hover:underline";

  return (
    <button className={linkClass} onClick={handleLogout}>
      Log out
    </button>
  );
};

export default LogoutButton;
