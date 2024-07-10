import axios, { AxiosResponse } from "axios";
import { getToken } from "../Config/HandelJWT";

//Axios Get Request
export async function axiosGet(Props: AxiosApiCallProps) {
  const authHeader = {
    headers: { Authorization: `Bearer ${getToken()}` },
  };
  return axios
    .get(Props.url, authHeader)
    .then((response) => Props.onSuccess(response.data))
    .catch((error) => Props.onError(error.response.data));
}

//Axios Post Request
export async function axiosPost(Props: AxiosApiCallProps) {
  const authHeader = {
    headers: { Authorization: `Bearer ${getToken()}` },
  };
  let requestHeaders = Props.isAuth ? authHeader : undefined;
  axios
    .post(Props.url, Props.data, requestHeaders)
    .then((response) => Props.onSuccess(response))
    .catch((error) => Props.onError(error.response));
}

//Axios Put Request
export async function axiosPut(Props: AxiosApiCallProps) {
  const authHeader = {
    headers: { Authorization: `Bearer ${getToken()}` },
  };
  let requestHeaders = Props.isAuth ? authHeader : undefined;
  axios
    .put(Props.url, Props.data, requestHeaders)
    .then((response) => Props.onSuccess(response))
    .catch((error) => Props.onError(error.response));
}

//Axios Delete Request
export async function axiosDelete(Props: AxiosApiCallProps) {
  const authHeader = {
    headers: { Authorization: `Bearer ${getToken()}` },
  };
  axios
    .delete(Props.url, authHeader)
    .then((response) => Props.onSuccess(response))
    .catch((error) => Props.onError(error.response));
}

interface AxiosApiCallProps {
  url: string;
  data?: {};
  onSuccess(response: AxiosResponse): void;
  onError(error: AxiosResponse): void;
  isAuth?: boolean;
}
