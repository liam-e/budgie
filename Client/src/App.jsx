import {
  Route,
  createBrowserRouter,
  createRoutesFromElements,
  RouterProvider,
  Navigate,
} from "react-router-dom";

import MainLayout from "./layouts/MainLayout";
import AuthLayout from "./layouts/AuthLayout";

import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import TransactionsPage, { transactionsLoader } from "./pages/TransactionsPage";
import NotFoundPage from "./pages/NotFoundPage";

const App = () => {
  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<MainLayout />}>
        <Route index element={<HomePage />} />
        <Route path="register" element={<RegisterPage />} />
        <Route path="login" element={<LoginPage />} />
        <Route
          path="home"
          element={<AuthLayout />}
          // errorElement={<h2>Error! something went wrong...</h2>}
        >
          <Route
            path="dashboard"
            element={<DashboardPage />}
            loader={transactionsLoader}
          />
          <Route index element={<Navigate to="dashboard" replace />} />
          <Route
            path="transactions"
            element={<TransactionsPage />}
            loader={transactionsLoader}
          />
        </Route>
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    )
  );
  return <RouterProvider router={router} />;
};

export default App;
