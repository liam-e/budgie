import {
  Route,
  createBrowserRouter,
  createRoutesFromElements,
  Navigate,
} from "react-router-dom";

import MainLayout from "./layouts/MainLayout";
import AuthLayout from "./layouts/AuthLayout";

import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import TransactionsPage, { transactionsLoader } from "./pages/TransactionsPage";
import CategoriesPage, { categoriesLoader } from "./pages/CategoriesPage";
import NotFoundPage from "./pages/NotFoundPage";
import AddDataPage from "./pages/AddDataPage";
import UploadCSVPage from "./pages/UploadCSVPage";
import AddFeedPage from "./pages/AddFeedPage";
import ManualEntryPage from "./pages/ManualEntryPage";
import BudgetLimitsPage from "./pages/BudgetLimitsPage";
import CategorizationsPage from "./pages/CategorizationsPage";

const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="login" element={<LoginPage />} />
      <Route path="register" element={<RegisterPage />} />

      <Route
        path="/"
        element={<MainLayout />}
        errorElement={<Navigate to="/" />}
      >
        <Route index element={<HomePage />} />

        <Route
          path="home"
          element={<AuthLayout />}
          loader={categoriesLoader}
          id="home"
        >
          <Route
            path="dashboard"
            element={<DashboardPage />}
            loader={transactionsLoader}
          />
          <Route
            path="transactions"
            element={<TransactionsPage />}
            loader={transactionsLoader}
          />
          <Route
            path="categories"
            element={<CategoriesPage />}
            loader={categoriesLoader}
          />
          <Route
            path="budget-limits"
            element={<BudgetLimitsPage />}
            loader={transactionsLoader}
          />

          <Route
            path="categorizations"
            element={<CategorizationsPage />}
            loader={transactionsLoader}
          />
          <Route path="add-data" element={<AddDataPage />} />
          <Route path="upload-csv" element={<UploadCSVPage />} />
          <Route path="add-feed" element={<AddFeedPage />} />
          <Route path="manual-entry" element={<ManualEntryPage />} />
          <Route index element={<Navigate to="dashboard" replace />} />
        </Route>
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </>
  )
);

export default router;
