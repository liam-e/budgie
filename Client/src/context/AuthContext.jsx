import { createContext, useContext, useEffect, useRef, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const getCookie = (key) => {
    const b = document.cookie.match("(^|;)\\s*" + key + "\\s*=\\s*([^;]+)");
    return b ? b.pop() : "";
  };

  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [userData, setUserData] = useState(() => {
    const storedUserData = localStorage.getItem("userData");
    return storedUserData ? JSON.parse(storedUserData) : null;
  });
  const [isRefreshing, setIsRefreshing] = useState(false);
  const hasRefreshTokenCached = useRef(getCookie("refreshToken") !== "");

  const logError = (message, error) => {
    console.error(`${message}:`, error);
  };

  const checkAuthStatus = async () => {
    setIsLoading(true);
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Auth/status`,
        {
          method: "GET",
          credentials: "include",
        }
      );

      if (response.ok) {
        setIsAuthenticated(true);
      } else {
        setIsAuthenticated(false);
        if (hasRefreshTokenCached.current) {
          await refresh();
        } else {
          console.log("No refresh token!");
        }
      }
    } catch (error) {
      logError("Error getting authentication status", error);
      setIsAuthenticated(false);
    } finally {
      setIsLoading(false);
    }
  };

  const refresh = async () => {
    if (isRefreshing) return;
    setIsRefreshing(true);

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Auth/refresh`,
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
        localStorage.removeItem("userData");
      }
    } catch (error) {
      logError("Error refreshing token", error);
      setIsAuthenticated(false);
      localStorage.removeItem("userData");
    } finally {
      setIsRefreshing(false);
    }
  };

  const login = async (user) => {
    setIsLoading(true);
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Auth/login`,
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
      return data.message;
    } catch (error) {
      setIsAuthenticated(false);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const signUp = async (newUser) => {
    setIsLoading(true);
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Auth/register`,
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

      return data.message;
    } catch (error) {
      setIsAuthenticated(false);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Auth/logout`,
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
      logError("Error logging out", error);
    }
  };

  useEffect(() => {
    let isMounted = true;
    const performCheck = async () => {
      if (isMounted) await checkAuthStatus();
    };
    performCheck();

    return () => {
      isMounted = false;
    };
  }, []);

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
