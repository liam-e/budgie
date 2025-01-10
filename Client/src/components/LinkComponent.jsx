import { Link } from "react-router-dom";

const LinkComponent = ({ children, ...props }) => {
  return (
    <Link className="link" {...props}>
      {children}
    </Link>
  );
};

export default LinkComponent;
