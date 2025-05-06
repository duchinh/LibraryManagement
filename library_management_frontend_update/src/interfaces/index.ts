export * from './auth.interface';
export * from './user.interface';
export * from './book.interface';

export interface Category {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
  updatedAt: string;
  bookCount?: number;
}

export interface Book {
  id: string;
  title: string;
  author: string;
  isbn: string;
  categoryId: string;
  quantity: number;
  availableQuantity: number;
  status: 'Available' | 'Unavailable' | 'Borrowed' | 'Reserved' | 'Lost';
  createdAt: string;
  updatedAt: string;
  categoryName?: string;
}

export interface BookReview {
  id: string;
  bookId: string;
  userId: string;
  rating: number;
  comment?: string;
  createdAt: string;
  updatedAt: string;
  book?: Book;
  user?: User;
}

export interface BookBorrowingRequest {
  id: string;
  requestorId: string;
  requestor?: User;
  approverId?: string;
  approver?: User;
  status: 'WAITING' | 'APPROVED' | 'REJECTED';
  dateRequested: string;
  dateApproved?: string;
  details: BookBorrowingRequestDetail[];
  createdAt: string;
  updatedAt: string;
}

export interface BookBorrowingRequestDetail {
  id: string;
  requestId: string;
  bookId: string;
  book?: Book;
  status: 'BORROWED' | 'RETURNED' | 'OVERDUE';
  borrowDate?: string;
  returnDate?: string;
  extendedDate?: string;
  createdAt: string;
  updatedAt: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export type { LoginResponse } from './auth.interface';

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface User {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  phoneNumber?: string;
  role: 'SuperUser' | 'NormalUser';
  isActive: boolean;
  lastLoginDate?: string;
  createdAt: string;
  updatedAt?: string;
  isDeleted: boolean;
}

export interface BorrowRequest {
  id: string;
  bookIds: number[];
  borrowDate: string;
  dueDate: string;
  notes?: string;
  status: 'Pending' | 'Approved' | 'Rejected';
  requestorId: string;
  requestorName: string;
  requestDate: string;
  approverId?: string;
  approverName?: string;
  bookCount: number;
  details: BorrowRequestDetail[];
}

export interface BorrowRequestDetail {
  id: string;
  bookId: string;
  bookTitle: string;
  quantity: number;
}

export interface BorrowedBook {
  id: string;
  bookId: string;
  title: string;
  author: string;
  categoryName: string;
  borrowDate: string;
  dueDate: string;
  status: 'Borrowed' | 'Overdue';
}

export interface DashboardStats {
  totalBooks: number;
  totalUsers: number;
  pendingRequests: number;
  overdueBooks: number;
} 