import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import store from "../store";
import { setLoginDetails, setSignOut } from "../../features/login/loginSlice";

axios.interceptors.request.use((config) => {
  const accessToken = store.getState().login.accessToken;
  if (accessToken) config.headers.Authorization = `Bearer ${accessToken}`;
  return config;
});

axios.interceptors.response.use(
  (response) => {
    // Do something with the response data, such as transforming or validating it
    return response;
  },
  (error: AxiosError) => {
    // Do something with the response error, such as logging or displaying a message
    console.error(error);
    if (error.response) {
      const { data, status, config } = error.response!;
      if (status === 401) {
        //Get the original request
        const originalRequest = error.config;
        //Get the refresh token
        const refreshToken = store.getState().login.refreshToken;
        // Check if there is a refresh token and if the original request has not been retried before
        if (refreshToken && refreshToken !== "" && !originalRequest!._retry) {
          // Set a flag to indicate that the request has been retried
          originalRequest!._retry = true;
          // Send a request to the refresh token endpoint with the refresh token and the expired access token
          try {
            const retriedResponsePromise = axios
              .post(
                "https://localhost:7033/connect/token",
                {
                  refresh_token: refreshToken,
                  grant_type: "refresh_token",
                  client_id: "postman",
                  client_secret: "postman-secret",
                },
                {
                  headers: {
                    "Content-Type": "application/x-www-form-urlencoded",
                  },
                }
              )
              .then((res) => {
                // Get the new access token from the response
                const newAccessToken: string = res.data.access_token;
                const newRefreshToken: string = res.data.refresh_token;

                // Update the access token in the store
                store.dispatch(
                  setLoginDetails({
                    accessToken: newAccessToken,
                    refreshToken: newRefreshToken,
                  })
                );

                // Update the authorization header of the original request with the new access token
                originalRequest!.headers.Authorization = `Bearer ${newAccessToken}`;
                // Retry the original request with the new access token
                const response = Promise.resolve(axios(originalRequest!)); // or const response = await axios(originalRequest!)
                return response;
              });

            //return await retriedResponsePromise;
            return Promise.resolve(retriedResponsePromise);
          } catch (err) {
            // Handle any errors that occur during refreshing the token, such as invalid or expired refresh token
            console.error(err);
            // Redirect the user to the login page or show a message asking them to re-authenticate
            return Promise.reject(err);
          }
        } else {
          console.error(error);
          return Promise.reject(error);
        }
      } // Return or throw the error for any other cases return Promise.reject(error); } );
      else if (status === 400) {
        if ((data as any).error === "invalid_grant") {
          store.dispatch(setSignOut({}));
        }
      } else {
        console.error(error);
        return Promise.reject(error);
      }
    }
  }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

export const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  postWithHeaders: <T>(url: string, body: {}, headers: {}) =>
    axios.post<T>(url, body, { headers: headers }).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};
