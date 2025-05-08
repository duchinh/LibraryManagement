import { httpClient } from "../http-services/axios/httpClient";
import { ENDPOINT_API } from "../http-services/axios/config";
import {
  AuthResponseDto,
  LoginRequestDto,
  RegisterRequestDto,
} from "../interfaces/auth.interface";

export const login = async (data: LoginRequestDto): Promise<AuthResponseDto> => {
  const response = await httpClient.post<AuthResponseDto>(
    ENDPOINT_API.auth.login,
    data
  );
  if (response.data.token) {
    localStorage.setItem("token", response.data.token);
  }
  return response.data;
};

export const register = async (
  data: RegisterRequestDto
): Promise<AuthResponseDto> => {
  const response = await httpClient.post<AuthResponseDto>(
    ENDPOINT_API.auth.register,
    data
  );
  return response.data;
};

export const logout = () => {
  localStorage.removeItem("token");
  window.location.href = "/login";
};
