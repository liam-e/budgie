import NavLinkComponent from "./NavLinkComponent";
import { useAuth } from "../context/AuthContext";
import UserDropdown from "./UserDropdown";
import { Link } from "react-router-dom";
import logo from "../assets/images/logo.png";

const Navbar = () => {
  const { isAuthenticated } = useAuth();

  const mainNav = (
    <>
      <Link
        to="/register"
        className="bg-pastelGreen text-black border-2 border-black p-3 no-underline hover:text-black hover:bg-pastelLightGreen"
      >
        Get Started
      </Link>
      <NavLinkComponent to="/login">Log in</NavLinkComponent>
    </>
  );

  const userNav = (
    <>
      <NavLinkComponent to="/home/dashboard">Dashboard</NavLinkComponent>
      <NavLinkComponent to="/home/transactions">Transactions</NavLinkComponent>
      <UserDropdown />
    </>
  );

  return (
    <div className="container mx-auto max-w-6xl px-6">
      <div className="flex h-20 items-center justify-between">
        <div className="flex flex-1 items-center">
          {/* Logo */}
          <Link to="/">
            <div className="flex flex-row items-center hidden md:flex text-pastelDarkGreen text-3xl font-normal">
              <img src={logo} alt="Logo" className="h-12 w-12" />
              <span>Budgie</span>
            </div>
          </Link>
        </div>
        {/* Navigation Links */}
        <div className="flex space-x-2 items-center">
          {isAuthenticated ? userNav : mainNav}
        </div>
      </div>
    </div>
  );
};

export default Navbar;
