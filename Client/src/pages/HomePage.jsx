import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

import HeroSection from "../components/HeroSection";
import Loading from "../components/Loading";
import FeaturesSection from "../components/FeaturesSection";

const HomePage = () => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return <Loading />;
  }

  if (isAuthenticated) {
    return <Navigate to="/home/dashboard" replace />;
  }

  return (
    <div className="w-full">
      <div className="container max-w-6xl mx-auto">
        <HeroSection />
        <FeaturesSection />
      </div>
    </div>
  );
};

export default HomePage;
