import { BankOutlined, UserOutlined } from '@ant-design/icons';
import { MenuDataItem, ProLayout } from '@ant-design/pro-components';
import { PropsWithChildren } from 'react';
import { useNavigate } from 'react-router-dom';

const defaultMenus: MenuDataItem[] = [
  {
    path: 'customers/list',
    name: 'Clientes',
    icon: <UserOutlined />,
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
        height: '100vh',
      }}
      onMenuHeaderClick={() => navigate('/')}
      title='Bankly'
      logo={<BankOutlined />}
      route={{
        path: '/',
        children: defaultMenus,
      }}
    >
      {children}
    </ProLayout>
  );
}

export default AppLayout;
