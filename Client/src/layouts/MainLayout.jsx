import { Outlet } from "react-router-dom";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

import { useLocation } from "react-router-dom";
import { useEffect } from "react";

const MainLayout = () => {
  const location = useLocation();

  const centeredPaths = new Set([
    "/home/add-data",
    "/home/upload-csv",
    "/home/add-feed",
    "/home/manual-entry",
  ]);

  const isCentered =
    centeredPaths.has(location.pathname) ||
    location.pathname.startsWith("/home/transactions/");

  useEffect(() => {
    console.log(isCentered);
  }, [isCentered]);

  return (
    <div className="flex flex-col min-h-screen">
      <Navbar />
      <div
        className={`flex-grow container mx-auto max-w-6xl px-6 ${
          isCentered ? "flex items-center justify-center" : ""
        }`}
      >
        <Outlet />
      </div>
      <Footer />
    </div>
  );
};

export default MainLayout;
