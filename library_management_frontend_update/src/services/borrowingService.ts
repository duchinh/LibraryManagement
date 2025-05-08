import { ENDPOINT_API } from "../http-services/axios/config";
import { httpClient } from "../http-services/axios/httpClient";
import {
CreateBorrowingRequestDTO,
  BorrowingRequestDTO,
  RequestDetailsDto,
} from "../interfaces/borrow.interface";

export const borrowBooks = async (
  data: CreateBorrowingRequestDTO
): Promise<CreateBorrowingRequestDTO> => {
  const response = await httpClient.post<CreateBorrowingRequestDTO>(
    ENDPOINT_API.borrowings.borrow,
    data
  );
  return response.data;
};

export const getMyBorrowRequests = async (
  userId: string
): Promise<BorrowingRequestDTO[]> => {
  try {
    const response = await httpClient.get<BorrowingRequestDTO[]>(
      `${ENDPOINT_API.borrowings.getByUserId.replace("{id}", userId)}`
    );
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch borrow requests");
  }
};

export const getAllBorrowRequests = async (): Promise<
  BorrowingRequestDTO[]
> => {
  try {
    const response = await httpClient.get<BorrowingRequestDTO[]>(
      ENDPOINT_API.borrowings.getAllRequests
    );
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch all borrow requests");
  }
};

export const updateBorrowRequestStatus = async (
  requestId: string,
  status: number
): Promise<void> => {
  try {
    await httpClient.put(
      ENDPOINT_API.borrowings.updateStatus.replace("{id}", requestId),
      { status }
    );
  } catch (error) {
    throw new Error("Failed to update borrow request status");
  }
};

export const getBorrowRequestDetails = async (
  requestId: string
): Promise<RequestDetailsDto> => {
  try {
    const response = await httpClient.get<RequestDetailsDto>(
      ENDPOINT_API.borrowings.getByRequestId.replace("{requestId}", requestId)
    );
    return response.data;
  } catch (error) {
    throw new Error("Failed to fetch borrow request details");
  }
};
