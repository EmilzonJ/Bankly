import { MenuDataItem, ProLayout } from "@ant-design/pro-components";
import { PropsWithChildren } from "react";
import { BankOutlined, UserOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

const defaultMenus: MenuDataItem[] = [
  {
    path: "customers",
    name: "Clientes",
    icon: <UserOutlined />,
    children: [
      {
        path: "list",
        name: "Listado de clientes",
      },
    ],
  },
];

function AppLayout({ children }: PropsWithChildren) {
  const navigate = useNavigate();

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
    >
      {children}
    </ProLayout>
  );
}

export default AppLayout;
