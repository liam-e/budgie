import NavLinkComponent from "./NavLinkComponent";
import { useAuth } from "../context/AuthContext";
import LogoutButton from "./LogoutButton";

const Navbar = () => {
  const { isAuthenticated } = useAuth();

  const mainNav = <NavLinkComponent to="/login">Log in</NavLinkComponent>;

  const userNav = (
    <>
      <NavLinkComponent to="/home/dashboard">Dashboard</NavLinkComponent>
      <NavLinkComponent to="/home/transactions">Transactions</NavLinkComponent>
      <LogoutButton />
    </>
  );

  return (
    <nav className="bg-pastelYellow border-b-4 border-pastelGreen">
      <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
        <div className="flex h-20 items-center justify-between">
          <div className="flex flex-1 items-center">
            {/* <!-- Logo --> */}
            <NavLinkComponent to="/">
              <span className="hidden md:block text-pastelGreen text-3xl font-normal ml-2">
                🐦Budgie
              </span>
            </NavLinkComponent>
          </div>
          {/* Navigation Links */}
          <div className="flex space-x-4 items-center">
            {isAuthenticated ? userNav : mainNav}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
