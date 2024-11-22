import React, { useState, useEffect, useRef } from "react";
import { FaChevronDown, FaUser } from "react-icons/fa";
import LogoutButton from "./LogoutButton";
import NavLinkComponent from "./NavLinkComponent";
import { useAuth } from "../context/AuthContext";

const UserDropdown = () => {
  const { userData } = useAuth();
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef(null);

  const toggleDropdown = () => {
    setIsOpen((prev) => !prev);
  };

  const handleClickOutside = (event) => {
    if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
      setIsOpen(false);
    }
  };

  useEffect(() => {
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div className="relative inline-block" ref={dropdownRef}>
      {/* Icon that triggers the dropdown */}
      <button
        onClick={toggleDropdown}
        className="flex items-center space-x-2 bg-pastelGreen hover:bg-pastelLightGreen border-2 border-black p-3 text-sm"
      >
        <FaUser className="text-xl text-black" />
        {userData && <span>{userData.firstName}</span>}
        <FaChevronDown />
      </button>

      {/* Dropdown Menu */}
      {isOpen && (
        <div className="absolute left-0 bg-pastelYellow border-2 border-black w-36">
          <ul className="flex flex-col space-y-2 p-2 justify-center">
            {/* <li>
              <NavLinkComponent to="/home/settings">Settings</NavLinkComponent>
            </li> */}
            <li>
              <NavLinkComponent to="/home/add-data">Add data</NavLinkComponent>
            </li>
            <li>
              <NavLinkComponent to="/home/categories">
                Categories
              </NavLinkComponent>
            </li>
            <li>
              <LogoutButton />
            </li>
          </ul>
        </div>
      )}
    </div>
  );
};

export default UserDropdown;
