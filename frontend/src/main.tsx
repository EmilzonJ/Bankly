import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';

import { Provider } from 'react-redux';
import { store } from './core/store/rtk.store.ts';
import Alert from './globals/alert.ts';
import ThemeProvider from './core/providers/ThemeProvider.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ThemeProvider>
      <Alert />
      <Provider store={store}>
        <App />
      </Provider>
    </ThemeProvider>
  </React.StrictMode>
);
