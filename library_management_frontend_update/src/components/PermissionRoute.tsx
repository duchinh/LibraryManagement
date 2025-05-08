import { Navigate } from "react-router-dom";
import { useAuthContext } from "../contexts/AuthContext";
import { JSX } from "react";

interface PermissionRouteProps {
  allowedRoles: string[];
  children: JSX.Element;
}

const PermissionRoute = ({ allowedRoles, children }: PermissionRouteProps) => {
  const { isLoading, userRole } = useAuthContext();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!allowedRoles.includes(userRole || "")) {
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
};

export default PermissionRoute;
