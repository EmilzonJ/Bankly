import { useAuth } from "@/core/hooks/useAuth";
import {
  BankOutlined,
  BookOutlined,
  LogoutOutlined,
  UserOutlined,
} from "@ant-design/icons";
import { MenuDataItem, ProLayout } from "@ant-design/pro-components";
import { Dropdown } from "antd";
import { PropsWithChildren, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const defaultMenus: MenuDataItem[] = [
  {
    path: "customers/list",
    name: "Clientes",
    icon: <UserOutlined />,
  },
  {
    path: "accounts/list",
    name: "Cuentas",
    icon: <BookOutlined />,
  },
  {
    path: "transactions",
    name: "Transacciones",
    icon: <BankOutlined />,
    children: [
      {
        path: "list",
        name: "Todas las Transacciones",
      },
      {
        path: "new",
        name: "Nueva Transacción",
      },
    ],
  },
];

function AppLayout({ children }: PropsWithChildren) {
  const navigate = useNavigate();
  const { user } = useAuth();

  useEffect(() => {
    if (!user) {
      navigate("/login");
    }
  }, [user, navigate]);

  return (
    <ProLayout
      menuProps={{
        onSelect: (e) => {
          navigate(e.key);
        },
      }}
      style={{
        height: "100vh",
      }}
      onMenuHeaderClick={() => navigate("/")}
      title="Bankly"
      logo={<BankOutlined />}
      route={{
        path: "/",
        children: defaultMenus,
      }}
      avatarProps={{
        src: "https://gw.alipayobjects.com/zos/antfincdn/efFD%24IOql2/weixintupian_20170331104822.jpg",
        size: "small",
        title: `${user?.email}`,
        render: (_, dom) => {
          return (
            <Dropdown
              menu={{
                items: [
                  {
                    key: "logout",
                    icon: <LogoutOutlined />,
                    label: "Cerrar Sesión",
                  },
                ],
              }}
            >
              {dom}
            </Dropdown>
          );
        },
      }}
    >
      {children}
    </ProLayout>
  );
}

export default AppLayout;
