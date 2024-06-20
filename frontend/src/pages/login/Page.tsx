import { useLoginMutation } from "@/core/features/auth/api/auth-api.slice";
import LoginPresentation from "@/core/features/auth/components/LoginPresentation";
import { useAuth } from "@/core/hooks/useAuth";
import { Login } from "@/core/types/auth.type";
import { App } from "antd";
import { useNavigate } from "react-router-dom";

function LoginPage() {
  const [loginMutation] = useLoginMutation();
  const navigate = useNavigate();
  const { login } = useAuth();

  const { notification } = App.useApp();

  const onSubmit = async (values: Login) => {
    const response = await loginMutation({ ...values }).unwrap();

    login(response.email, response.token);

    notification.success({ message: `Bienvenido ${response.email}` });
    navigate("/");
  };

  return (
    <div style={{ display: "flex", height: "100svh", alignItems: "center" }}>
      <LoginPresentation onSubmit={onSubmit} />
    </div>
  );
}

export default LoginPage;
