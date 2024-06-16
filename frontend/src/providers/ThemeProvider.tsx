import { ConfigProvider } from "antd";
import { PropsWithChildren } from "react";
import esEs from "antd/locale/es_ES";

const data = {
  borderRadius: 6,
  colorPrimary: "#9E339F",
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
      {children}
    </ConfigProvider>
  );
}

export default ThemeProvider;
