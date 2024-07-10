import { authenticationResponse, claim } from "../utils/types/AuthenticationTypes";

const tokenkey = "token";
const expirationKey = "expiration";

export function saveToken(authData: authenticationResponse) {
  localStorage.setItem(tokenkey, authData.token);
  localStorage.setItem(expirationKey, authData.expiration.toString());
}

export function getToken() {
  const token = localStorage.getItem(tokenkey);
  const expiration = localStorage.getItem(expirationKey)!;
  const expirationDate = new Date(expiration);
  if (!token) {
    return null;
  }
  if (expirationDate <= new Date()) {
    logout();
    return null;
  }
  return token;
}

export function getClaims(): claim[] {
  const token = localStorage.getItem(tokenkey);

  if (!token) {
    return [];
  }

  const expiration = localStorage.getItem(expirationKey)!;

  const expirationDate = new Date(expiration);

  if (expirationDate <= new Date()) {
    logout();
    return [];
  }

  const tokenData = JSON.parse(atob(token.split(".")[1]));
  const response = [];
  for (const property in tokenData) {
    response.push({ name: property, value: tokenData[property] });
  }

  return response;
}

export function logout() {
  localStorage.removeItem(tokenkey);
  localStorage.removeItem(expirationKey);
}
