import { Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import Loading from "../components/Loading";
import MessageContainer from "../components/MessageContainer";

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
    <div className="container mx-auto max-w-6xl min-w-sm py-4">
      <Outlet />
      <MessageContainer />
    </div>
  );
};

export default AuthLayout;
