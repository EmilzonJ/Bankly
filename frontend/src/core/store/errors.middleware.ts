/* eslint-disable @typescript-eslint/ban-ts-comment */
import { message } from '@/globals/alert';
import { isRejectedWithValue, Middleware } from '@reduxjs/toolkit';

export const ErrorLoggerMiddleware: Middleware = () => (next) => (action) => {
  if (isRejectedWithValue(action)) {
    //@ts-ignore
    if (action.payload.response?.data?.StatusCode >= 500) {
      message.error(
        'Ocurrió un error en el servidor, por favor intente más tarde'
      );
    }
  }
  //@ts-ignore
  if (action.payload?.status >= 400) {
    console.log(action.payload.data.errors);
    console.log(message);

    (action.payload.data.errors as string[])?.forEach((e) => {
      message.error(e.description);
    });
  }

  return next(action);
};
