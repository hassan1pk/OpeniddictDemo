import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import store from "../store";

axios.interceptors.request.use((config) => {
  const token = store.getState().login.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  (response) => {
    // Do something with the response data, such as transforming or validating it
    return response;
  },
  (error) => {
    // Do something with the response error, such as logging or displaying a message
    console.error(error);
    return Promise.reject(error);
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

export const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};
