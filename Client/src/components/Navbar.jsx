import NavLinkComponent from "./NavLinkComponent";
import { useAuth } from "../context/AuthContext";
import UserDropdown from "./UserDropdown";
import { Link } from "react-router-dom";

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
    <nav className="bg-pastelYellow border-b-2 border-black">
      <div className="container mx-auto max-w-6xl px-6">
        <div className="flex h-20 items-center justify-between">
          <div className="flex flex-1 items-center">
            {/* Logo */}
            <Link className="no-underline" to="/">
              <span className="hidden md:block text-pastelGreen text-3xl font-normal">
                🐦Budgie
              </span>
            </Link>
          </div>
          {/* Navigation Links */}
          <div className="flex space-x-2 items-center">
            {isAuthenticated ? userNav : mainNav}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
