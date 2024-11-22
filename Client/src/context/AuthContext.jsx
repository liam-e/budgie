import { createContext, useContext, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [userData, setUserData] = useState(() => {
    // Load initial userData from localStorage if available
    const storedUserData = localStorage.getItem("userData");
    return storedUserData ? JSON.parse(storedUserData) : null;
  });

  const getCookie = (key) => {
    const b = document.cookie.match("(^|;)\\s*" + key + "\\s*=\\s*([^;]+)");
    return b ? b.pop() : "";
  };

  const hasRefreshToken = () => getCookie("refreshToken") !== "";

  const checkAuthStatus = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/auth/status`,
        {
          method: "GET",
          credentials: "include",
          headers: { "Content-Type": "application/json" },
        }
      );

      if (response.ok) {
        setIsAuthenticated(true);
      } else {
        setIsAuthenticated(false);
        if (hasRefreshToken()) {
          await refresh();
        } else {
          console.log("No refresh token!");
        }
      }
    } catch (error) {
      console.error("Error getting authentication status", error);
      setIsAuthenticated(false);
    } finally {
      setIsLoading(false);
    }
  };

  const refresh = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/auth/refresh`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        setIsAuthenticated(true);
      } else {
        console.error("Refresh failed with status:", response.statusText);
        setIsAuthenticated(false);
      }
    } catch (error) {
      console.error("Error refreshing token:", error);
      setIsAuthenticated(false);
    }
  };

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const login = async (user) => {
    setIsLoading(true);
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/auth/login`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(user),
        }
      );

      if (!response.ok) throw new Error(response.statusText);

      const data = await response.json();

      setIsAuthenticated(true);
      setUserData(data.user);
      localStorage.setItem("userData", JSON.stringify(data.user));
      setIsLoading(false);
      return data.message;
    } catch (error) {
      setIsAuthenticated(false);
      setIsLoading(false);
      throw error;
    }
  };

  const signUp = async (newUser) => {
    setIsLoading(true);
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/auth/register`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(newUser),
        }
      );

      if (!response.ok) {
        const error = new Error(response.statusText);
        error.status = response.status;
        throw error;
      }

      const data = await response.json();

      setIsAuthenticated(true);
      setUserData(data.user);
      localStorage.setItem("userData", JSON.stringify(data.user));

      setTimeout(() => {
        setIsLoading(false);
      }, 100);

      return data.message;
    } catch (error) {
      setIsAuthenticated(false);
      setIsLoading(false);
      throw error;
    }
  };

  const logout = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/auth/logout`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (!response.ok) throw new Error(response.statusText);

      setIsAuthenticated(false);
      setUserData(null);
      localStorage.removeItem("userData");
    } catch (error) {
      console.error("Error logging out:", error);
    }
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        setIsAuthenticated,
        setIsLoading,
        isLoading,
        userData,
        login,
        logout,
        refresh,
        signUp,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
