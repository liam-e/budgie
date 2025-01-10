import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import RegisterForm from "../forms/RegisterForm";
import { useEffect } from "react";

const RegisterPage = () => {
  const { isAuthenticated, isLoading } = useAuth();

  useEffect(() => {
    if (isAuthenticated && !isLoading) {
      return <Navigate to="/home/dashboard" replace />;
    }
  }, []);

  return (
    <div className="centerboxparent h-screen">
      <div className="centerboxchild colorbox p-8">
        <h2 className="pageheading text-center">Sign Up</h2>

        <RegisterForm />
      </div>
    </div>
  );
};

export default RegisterPage;
