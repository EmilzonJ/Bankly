import { configureStore } from '@reduxjs/toolkit';
import { baseApi } from './base.api';
import { ErrorLoggerMiddleware } from './errors.middleware';

export const store = configureStore({
  reducer: {
    [baseApi.reducerPath]: baseApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(ErrorLoggerMiddleware, baseApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
