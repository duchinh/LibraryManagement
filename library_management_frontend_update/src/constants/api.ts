import axios from 'axios';
import { Book } from '../interfaces/book.interface';
import { BorrowRequest, BorrowRequestCreateDTO, BorrowRequestUpdateDTO } from '../interfaces/borrow.interface';
import { User, UserCreateDTO, UserUpdateDTO } from '../interfaces/user.interface';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../interfaces/auth.interface';

export const API_URL = 'http://localhost:5000/api';

const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Thêm interceptor để thêm token vào header
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Thêm interceptor để xử lý refresh token
axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await axios.post(`${API_URL}/auth/refresh-token`, { refreshToken });
        const { accessToken } = response.data;
        localStorage.setItem('token', accessToken);
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return axiosInstance(originalRequest);
      } catch (error) {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        window.location.href = '/login';
        return Promise.reject(error);
      }
    }
    return Promise.reject(error);
  }
);

// Types
export interface ReportData {
  totalBorrows: number;
  totalReturns: number;
  totalOverdue: number;
  totalFines: number;
  details: {
    date: string;
    borrows: number;
    returns: number;
    overdue: number;
    fines: number;
  }[];
}

export interface Report {
  type: string;
  data: ReportData;
  generatedAt: string;
}

export interface DashboardStats {
  totalBooks: number;
  totalUsers: number;
  pendingRequests: number;
  overdueBooks: number;
}

export const api = {
  auth: {
    login: (data: LoginRequest) => axiosInstance.post<LoginResponse>('/auth/login', data),
    register: (data: RegisterRequest) => axiosInstance.post<RegisterResponse>('/auth/register', data),
    refreshToken: (refreshToken: string) => axiosInstance.post('/auth/refresh', { refreshToken }),
    logout: (refreshToken: string) => axiosInstance.post('/auth/logout', { refreshToken }),
    changePassword: (data: { currentPassword: string; newPassword: string }) => 
      axiosInstance.post('/auth/change-password', data)
  },
  books: {
    getAll: () => axiosInstance.get<Book[]>('/books'),
    getById: (id: number) => axiosInstance.get<Book>(`/books/${id}`),
    create: (data: Omit<Book, 'id' | 'createdAt' | 'updatedAt'>) => axiosInstance.post<Book>('/books', data),
    update: (id: number, data: Partial<Book>) => axiosInstance.put<Book>(`/books/${id}`, data),
    delete: (id: number) => axiosInstance.delete(`/books/${id}`)
  },
  borrowRequests: {
    getAll: () => axiosInstance.get<BorrowRequest[]>('/borrow-requests'),
    getById: (id: number) => axiosInstance.get<BorrowRequest>(`/borrow-requests/${id}`),
    create: (data: BorrowRequestCreateDTO) => axiosInstance.post<BorrowRequest>('/borrow-requests', data),
    update: (id: number, data: BorrowRequestUpdateDTO) => axiosInstance.put<BorrowRequest>(`/borrow-requests/${id}`, data),
    delete: (id: number) => axiosInstance.delete(`/borrow-requests/${id}`),
    getMyRequests: () => axiosInstance.get<BorrowRequest[]>('/borrow-requests/my-requests'),
    canBorrowMore: () => axiosInstance.get<boolean>('/borrow-requests/can-borrow-more'),
    getBorrowingCount: () => axiosInstance.get<number>('/borrow-requests/borrowing-count'),
    returnBook: (id: number) => axiosInstance.post(`/borrow/${id}/return`)
  },
  users: {
    getAll: () => axiosInstance.get<User[]>('/users'),
    getById: (id: number) => axiosInstance.get<User>(`/users/${id}`),
    create: (data: UserCreateDTO) => axiosInstance.post<User>('/users', data),
    update: (id: number, data: UserUpdateDTO) => axiosInstance.put<User>(`/users/${id}`, data),
    delete: (id: number) => axiosInstance.delete(`/users/${id}`),
    getMyBooks: () => axiosInstance.get<Book[]>('/users/my-books')
  },
  admin: {
    getAllUsers: () => axiosInstance.get<User[]>('/api/admin/users'),
    changeUserRole: (id: string, role: string) => axiosInstance.put<User>(`/api/admin/users/${id}/role`, { role }),
    getReports: (type: string, startDate?: string, endDate?: string) => axiosInstance.get<Report>('/api/admin/reports', { params: { type, startDate, endDate } }),
    getDashboardStats: () => axiosInstance.get<DashboardStats>('/api/admin/dashboard')
  }
}; 