import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import LoginForm from "../forms/LoginForm";

const LoginPage = () => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/home/dashboard" replace />;
  }

  return (
    <div className="centerboxparent h-screen">
      <div className="centerboxchild colorbox p-8">
        <h2 className="pageheading text-center">Log in</h2>

        <LoginForm />
      </div>
    </div>
  );
};

export default LoginPage;
