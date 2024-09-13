import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  const getCookie = (key) => {
    var b = document.cookie.match("(^|;)\\s*" + key + "\\s*=\\s*([^;]+)");
    return b ? b.pop() : "";
  };

  const hasRefreshToken = () => {
    return getCookie("refreshToken") != "";
  };

  useEffect(() => {
    const status = async () => {
      return fetch(`${import.meta.env.VITE_API_URL}/auth/status`, {
        method: "GET",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
      })
        .then((response) => {
          if (response.ok) {
            setIsAuthenticated(true);
          } else {
            setIsAuthenticated(false);
            refresh();
          }
        })
        .catch((error) => {
          console.error("Error getting authentication status", error);
        });
    };

    status();
  }, []);

  useEffect(() => {
    if (!isAuthenticated && hasRefreshToken()) refresh();
  }, [isAuthenticated]);

  const login = async (user) => {
    return fetch(`${import.meta.env.VITE_API_URL}/auth/login`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(user),
    })
      .then((response) => {
        if (!response.ok) throw new Error(response.statusText);
        return response.json();
      })
      .then((data) => {
        setIsAuthenticated(true);
        return data;
      })
      .catch((error) => {
        setIsAuthenticated(false);
        throw error;
      });
  };

  const logout = async () => {
    return fetch(`${import.meta.env.VITE_API_URL}/auth/logout`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => {
        if (!response.ok) throw new Error(response.statusText);
        return response.json();
      })
      .then((data) => {
        // TODO: check that tokens have been expired/removed
        setIsAuthenticated(false);
        return data;
      });
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

      if (!response.ok) {
        console.error("Refresh HTTP status:", response.statusText);
      }

      return response;
    } catch (error) {
      console.error("Error refreshing token:", error);
      throw error;
    }
  };

  const register = async (newUser) => {
    try {
      const response = await fetch(`${apiUrl}/auth/register`, {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newUser),
      });

      if (!response.ok) {
        console.error("registerSubmit: error", response.statusText);
        setFormMessage("* The email already exists.");
      }

      await response.json();
    } catch (error) {
      console.error("registersubmit: Request failed", error);
      setFormMessage("* Something went wrong..");
    }
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        login,
        logout,
        refresh,
        register,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};
