import { ENDPOINT_API } from "../http-services/axios/config";
import { httpClient } from "../http-services/axios/httpClient";
import {
  CategoryDto,
  CreateCategoryDto,
  UpdateCategoryDto,
} from "../interfaces/category.interface";

export const getAllCategories = async (): Promise<CategoryDto[]> => {
  const response = await httpClient.get<CategoryDto[]>(
    ENDPOINT_API.categories.getAll
  );
  return response.data;
};

export const getCategoryById = async (id: string): Promise<CategoryDto> => {
  const response = await httpClient.get<CategoryDto>(
    ENDPOINT_API.categories.getById.replace("{id}", id)
  );
  return response.data;
};

export const createCategory = async (
  data: CreateCategoryDto
): Promise<CreateCategoryDto> => {
  const response = await httpClient.post<CreateCategoryDto>(
    ENDPOINT_API.categories.create,
    data
  );
  return response.data;
};

export const updateCategory = async (
  id: string,
  data: UpdateCategoryDto
): Promise<UpdateCategoryDto> => {
  const response = await httpClient.put<UpdateCategoryDto>(
    ENDPOINT_API.categories.update.replace("{id}", id),
    data
  );
  return response.data;
};

export const deleteCategory = async (id: string): Promise<void> => {
  await httpClient.delete(ENDPOINT_API.categories.delete.replace("{id}", id));
};
