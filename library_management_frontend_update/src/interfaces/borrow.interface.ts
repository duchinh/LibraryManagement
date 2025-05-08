export interface CreateBorrowingRequestDTO {
  userId: string;
  bookIds: string[];
  note?: string;
}

export interface BookBorrowingRequestResponseDto {
  id: string;
  userId: string;
  firstName: string;
  lastName: string;
  requestDate: string;
  approvedDate: string | null;
  returnedDate: string | null;
  status: number;
  note: string | null;
  bookTitles: string[];
}

export interface RequestDetailsDto {
  id: string;
  borrowedDate: string;
  dueDate: string | null;
  bookTitle: string;
}