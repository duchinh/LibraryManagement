export interface BorrowRequest {
  id: number;
  requestorId: number;
  requestorName: string;
  requestDate: string;
  borrowDate: string;
  dueDate: string;
  status: 'Pending' | 'Approved' | 'Rejected';
  notes?: string;
  approverId?: number;
  approverName?: string;
  bookCount: number;
  details: BorrowRequestDetail[];
  createdAt: string;
  updatedAt?: string;
}

export interface BorrowRequestDetail {
  id: number;
  bookId: number;
  bookTitle: string;
  borrowDate: string;
  dueDate: string;
  returnDate?: string;
  status: string;
  notes?: string;
}

export interface BorrowRequestCreateDTO {
  bookIds: number[];
  borrowDate: string;
  dueDate: string;
  notes?: string;
}

export interface BorrowRequestUpdateDTO {
  status: 'Approved' | 'Rejected';
  notes?: string;
} 