import { ENDPOINT_API } from "../http-services/axios/config";
import { httpClient } from "../http-services/axios/httpClient";
import { UserResponseDto, UserUpdateDto } from "../interfaces/user.interface";

export const getUserById = async (userId: string) => {
  try {
    const response = await httpClient.get<UserResponseDto>(
      ENDPOINT_API.users.getById.replace("{id}", userId)
    );
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch user data");
  }
};

export const getAllUsers = async () => {
  try {
    const response = await httpClient.get<UserResponseDto[]>(
      ENDPOINT_API.users.getAll
    );
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch users data");
  }
};

export const updateUser = async (userData: UserUpdateDto) => {
  try {
    const response = await httpClient.put<UserUpdateDto>(
      ENDPOINT_API.users.update,
      userData
    );
    return response.data;
  } catch (error: any) {
    if (error.response?.data?.errors) {
      const errorMessages = Object.entries(error.response.data.errors)
        .map(
          ([field, messages]) =>
            `${field}: ${(messages as string[]).join(", ")}`
        )
        .join("\n");
      throw new Error(errorMessages);
    }

    throw new Error("Failed to update user data");
  }
};

export const deleteUser = async (userId: string) => {
  try {
    const response = await httpClient.delete<UserResponseDto>(
      ENDPOINT_API.users.delete.replace("{id}", userId)
    );
    return response.data;
  } catch (error: any) {
    throw new Error(error.response?.data?.message || "Failed to delete user");
  }
};
