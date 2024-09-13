import { NavLink } from "react-router-dom";

const NavLinkComponent = ({ to, children }) => {
  const linkClass = ({ isActive }) =>
    `px-3 py-3 rounded-md text-sm font-medium ${
      isActive
        ? "text-pastelDarkGreen underline"
        : "text-pastelDarkGreen hover:text-pastelGreen no-underline hover:underline"
    }`;

  return (
    <NavLink to={to} className={linkClass}>
      {children}
    </NavLink>
  );
};

export default NavLinkComponent;
