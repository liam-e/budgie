import { NavLink } from "react-router-dom";

const NavLinkComponent = ({ to, children, onClick }) => {
  const linkClass = ({ isActive }) =>
    `px-3 py-3 rounded-md text-md font-normal ${
      isActive
        ? "text-pastelDarkGreen underline"
        : "text-pastelDarkGreen hover:text-pastelGreen no-underline hover:underline"
    }`;

  return (
    <NavLink to={to} className={linkClass} onClick={onClick}>
      {children}
    </NavLink>
  );
};

export default NavLinkComponent;
