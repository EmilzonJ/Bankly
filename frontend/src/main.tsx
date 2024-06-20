import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { Provider } from "react-redux";
import { store } from "./core/store/rtk.store.ts";
import Alert from "./globals/alert.ts";
import ThemeProvider from "./core/providers/ThemeProvider.tsx";
import { AuthProvider } from "./core/contexts/AuthContext.tsx";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <AuthProvider>
      <ThemeProvider>
        <Alert />
        <Provider store={store}>
          <App />
        </Provider>
      </ThemeProvider>
    </AuthProvider>
  </React.StrictMode>
);
