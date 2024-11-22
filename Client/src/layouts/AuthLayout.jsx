import { Navigate, Outlet, redirect, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import Loading from "../components/Loading";

const AuthLayout = () => {
  const { isAuthenticated, isLoading } = useAuth();
  const navigate = useNavigate();

  if (isLoading) {
    return <Loading />;
  }

  if (!isAuthenticated) {
    console.log("The user is not authenticated! Navigating to /login");
    return navigate("/login");
  }

  return (
    <div className="container mx-auto max-w-6xl p-6">
      <Outlet />
    </div>
  );
};

export default AuthLayout;
