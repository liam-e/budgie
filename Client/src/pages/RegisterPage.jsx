import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import RegisterForm from "../forms/RegisterForm";

const RegisterPage = () => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isAuthenticated && !isLoading) {
    return <Navigate to="/home/dashboard" replace />;
  }

  return (
    <div className="flex items-center justify-center min-h-screen">
      <div className="flex flex-col bg-pastelYellow px-6 py-4 border-4 border-pastelGreen">
        <h2 className="pageheading">Sign Up</h2>

        <RegisterForm />
      </div>
    </div>
  );
};

export default RegisterPage;
