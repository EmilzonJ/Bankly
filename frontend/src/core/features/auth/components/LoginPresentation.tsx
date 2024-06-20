import { Login } from "@/core/types/user.type";
import { BankFilled, LockOutlined, UserOutlined } from "@ant-design/icons";
import {
  LoginForm,
   ProFormText,
} from "@ant-design/pro-components";
import { Flex } from "antd";

interface LoginPresentationProps {
  onSubmit: (values: Login) => void;
}

const LoginPresentation = ({ onSubmit }: LoginPresentationProps) => {
  return (
    <LoginForm
      title={
        <Flex align="center" justify="center">
          <BankFilled
            style={{ fontSize: 30, marginRight: 10, color: "#1477FF" }}
          />
          <span style={{ color: "#1477FF" }}>Bankly</span>
        </Flex>
      }
      subTitle="Ingrese su correo y contraseña para iniciar sesión"
      onFinish={onSubmit}
      containerStyle={{height: "auto"}}
    >
      <ProFormText
        name="email"
        fieldProps={{
          type: "email",
          size: "large",
          prefix: <UserOutlined className={"prefixIcon"} />,
        }}
        placeholder="Digite su correo"
        rules={[
          {
            required: true,
            message: "¡Por favor ingrese su correo!",
          },
          {
            type: "email",
            message: "¡Por favor ingrese un correo válido!",
          },
        ]}
      />
      <ProFormText.Password
        name="password"
        fieldProps={{
          size: "large",
          prefix: <LockOutlined className={"prefixIcon"} />,
        }}
        placeholder="Digite su contraseña"
        rules={[
          {
            required: true,
            message: "¡Por favor ingrese su contraseña!",
          },
        ]}
      />
    </LoginForm>
  );
};

export default LoginPresentation;
