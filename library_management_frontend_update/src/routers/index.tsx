import AdminLayout from "../components/layout/AdminLayout";
import AppLayout from "../components/layout/AppLayout";
import PermissionRoute from "../components/PermissionRoute";
import ProtectedRoute from "../components/ProtectedRoute";
import AdminDashboard from "../pages/admin/AdminDashboard";
import BookManagePage from "../pages/books/BookManagePage";
import BorrowRequestPage from "../pages/borrow/BorrowRequestPage";
import BorrowRequestsManagePage from "../pages/borrow/BorrowRequestsManagePage";
import CategoryManagePage from "../pages/category/CategoryManagePage";
import HomePage from "../pages/home/HomePage";
import LoginPage from "../pages/auth/LoginPage";
import RegisterPage from "../pages/auth/RegisterPage";
import UnauthorizedPage from "../pages/auth/UnauthorizedPage";
import UserManagePage from "../pages/user/UserManagePage";

const pathname = {
  home: "/",
  login: "/login",
  register: "/register",
  borrowRequest: "/borrow-request",
  adminUser: "/admin/users",
  adminBook: "/admin/books",
  adminCategory: "/admin/categories",
  adminDashboard: "/admin/dashboard",
  adminRequest: "admin/requests",
  unauthorized: "/unauthorized",
};

export const AppRouters = [
  { path: pathname.login, element: <LoginPage /> },
  { path: pathname.register, element: <RegisterPage /> },
  { path: pathname.unauthorized, element: <UnauthorizedPage /> },
  {
    element: (
      <ProtectedRoute>
        <AppLayout />
      </ProtectedRoute>
    ),
    children: [
      { path: pathname.borrowRequest, element: <BorrowRequestPage /> },
      { path: pathname.home, element: <HomePage /> },
    ],
  },
  {
    element: (
      <ProtectedRoute>
        <PermissionRoute allowedRoles={["Admin"]}>
          <AdminLayout />
        </PermissionRoute>
      </ProtectedRoute>
    ),
    children: [
      { path: pathname.adminUser, element: <UserManagePage /> },
      { path: pathname.adminRequest, element: <BorrowRequestsManagePage /> },
      { path: pathname.adminDashboard, element: <AdminDashboard /> },
      { path: pathname.adminCategory, element: <CategoryManagePage /> },
      { path: pathname.adminBook, element: <BookManagePage /> },
    ],
  },
];
