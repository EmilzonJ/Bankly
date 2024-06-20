import { App, ConfigProvider } from 'antd';
import { PropsWithChildren } from 'react';
import esEs from 'antd/locale/es_ES';

const data = {
  borderRadius: 6,
  colorPrimary: '#1477FF',
};

function ThemeProvider({ children }: PropsWithChildren) {
  return (
    <ConfigProvider
      theme={{
        token: {
          colorPrimary: data.colorPrimary,
          borderRadius: data.borderRadius,
        },
      }}
      locale={esEs}

    >
      <App>{children}</App>
    </ConfigProvider>
  );
}

export default ThemeProvider;
