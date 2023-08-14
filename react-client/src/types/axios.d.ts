import "axios";

declare module "axios" {
  export interface AxiosRequestConfig {
    _retry?: boolean; // Add the _retry property as an optional boolean field
  }
}
