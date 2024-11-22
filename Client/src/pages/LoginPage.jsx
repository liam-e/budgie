import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import LoginForm from "../forms/LoginForm";

const LoginPage = () => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/home/dashboard" replace />;
  }

  return (
    <div className="flex items-center justify-center min-h-screen">
      <div className="flex flex-col bg-pastelYellow px-6 py-4 border-4 border-pastelGreen">
        <h2 className="mt-4 text-center">Log in</h2>

        <LoginForm />
      </div>
    </div>
  );
};

export default LoginPage;
