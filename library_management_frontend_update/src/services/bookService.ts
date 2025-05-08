import { ENDPOINT_API } from "../http-services/axios/config";
import { httpClient } from "../http-services/axios/httpClient";
import { BookDTO, CreateBookDTO, UpdateBookDTO } from "../interfaces/book.interface";

export const getAllBooks = async (): Promise<BookDTO[]> => {
  const response = await httpClient.get<BookDTO[]>(ENDPOINT_API.books.getAll);
  return response.data;
};

export const getBookById = async (id: string): Promise<BookDTO> => {
  const response = await httpClient.get<BookDTO>(
    ENDPOINT_API.books.getById.replace("{id}", id)
  );
  return response.data;
};

export const createBook = async (
  data: CreateBookDTO
): Promise<CreateBookDTO> => {
  const response = await httpClient.post<CreateBookDTO>(
    ENDPOINT_API.books.create,
    data
  );
  return response.data;
};

export const updateBook = async (
  id: string,
  data: UpdateBookDTO
): Promise<UpdateBookDTO> => {
  const response = await httpClient.put<UpdateBookDTO>(
    ENDPOINT_API.books.update.replace("{id}", id),
    data
  );
  return response.data;
};

export const deleteBook = async (id: string): Promise<void> => {
  await httpClient.delete(ENDPOINT_API.books.delete.replace("{id}", id));
};
