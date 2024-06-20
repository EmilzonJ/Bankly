import { Routes } from "@/routes/routes";
import { PropsWithChildren } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

function ProtectedRoute({ children }: PropsWithChildren) {
  const { user } = useAuth();

  if (!user) {
    return <Navigate to={Routes.LOGIN} />;
  }

  return children;
}

export default ProtectedRoute;
