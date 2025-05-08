import axios from 'axios';
import { Book } from '../interfaces/book.interface';
import { BorrowRequest, BorrowRequestCreateDTO } from '../interfaces/borrow.interface';
import { User, UserCreateDTO, UserUpdateDTO } from '../interfaces/user.interface';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../interfaces/auth.interface';
import { Category } from '../interfaces';

export const API_URL = 'http://localhost:5277';

const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Thêm interceptor để xử lý token
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
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        const refreshToken = localStorage.getItem('refreshToken');
        const response = await axios.post(`${API_URL}/api/auth/refresh`, { refreshToken });
        const { accessToken } = response.data;
        localStorage.setItem('token', accessToken);
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return axiosInstance(originalRequest);
      } catch (error) {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
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
    login: (data: LoginRequest) => axiosInstance.post<LoginResponse>('/api/auth/login', data),
    register: (data: RegisterRequest) => axiosInstance.post<RegisterResponse>('/api/auth/register', data),
    refreshToken: (refreshToken: string) => axiosInstance.post('/api/auth/refresh', { refreshToken }),
    logout: (refreshToken: string) => axiosInstance.post('/api/auth/logout', { refreshToken }),
    changePassword: (data: { currentPassword: string; newPassword: string }) => 
      axiosInstance.post('/api/auth/change-password', data)
  },
  books: {
    getAll: () => axiosInstance.get<Book[]>('/api/books'),
    getById: (id: number) => axiosInstance.get<Book>(`/api/books/${id}`),
    create: (data: Omit<Book, 'id' | 'createdAt' | 'updatedAt'>) => axiosInstance.post<Book>('/api/books', data),
    update: (id: number, data: Partial<Book>) => axiosInstance.put<Book>(`/api/books/${id}`, data),
    delete: (id: number) => axiosInstance.delete(`/api/books/${id}`),
    search: (searchTerm: string) => axiosInstance.get<Book[]>(`/api/books/search?searchTerm=${searchTerm}`),
    getByCategory: (categoryId: number) => axiosInstance.get<Book[]>(`/api/books/category/${categoryId}`)
  },
  categories: {
    getAll: () => axiosInstance.get<Category[]>('/api/categories'),
    getById: (id: number) => axiosInstance.get<Category>(`/api/categories/${id}`),
    create: (data: { name: string; description?: string }) => axiosInstance.post<Category>('/api/categories', data),
    update: (id: number, data: { name: string; description?: string }) => axiosInstance.put<Category>(`/api/categories/${id}`, data),
    delete: (id: number) => axiosInstance.delete(`/api/categories/${id}`)
  },
  borrowRequests: {
    getAll: () => axiosInstance.get<BorrowRequest[]>('/api/borrow-requests'),
    approveRequest: (id: number) => axiosInstance.put<BorrowRequest>(`/api/borrow-requests/${id}/approve`),
    rejectRequest: (id: number, reason: string) => axiosInstance.put<BorrowRequest>(`/api/borrow-requests/${id}/reject`, reason),
    create: (data: BorrowRequestCreateDTO) => axiosInstance.post<BorrowRequest>('/api/borrow-requests', data),
    getById: (id: number) => axiosInstance.get<BorrowRequest>(`/api/borrow-requests/${id}`),
    getMyRequests: () => axiosInstance.get<BorrowRequest[]>('/api/borrow-requests/user'),
    returnBooks: (id: number, bookIds: number[]) => axiosInstance.put(`/api/borrow-requests/${id}/return`, bookIds),
    canBorrowMore: () => axiosInstance.get<boolean>('/api/borrow-requests/can-borrow'),
    getBorrowingCount: () => axiosInstance.get<number>('/api/borrow-requests/borrowing-count')
  },
  users: {
    getAll: () => axiosInstance.get<User[]>('/api/users'),
    getById: (id: number) => axiosInstance.get<User>(`/api/users/${id}`),
    create: (data: UserCreateDTO) => axiosInstance.post<User>('/api/users', data),
    update: (id: number, data: UserUpdateDTO) => axiosInstance.put<User>(`/api/users/${id}`, data),
    delete: (id: number) => axiosInstance.delete(`/api/users/${id}`),
    getMyBooks: () => axiosInstance.get<Book[]>('/api/users/my-books')
  },
  admin: {
    getAllUsers: () => axiosInstance.get<User[]>('/api/admin/users'),
    changeUserRole: (id: string, role: string) => axiosInstance.put<User>(`/api/admin/users/${id}/role`, { role }),
    getReports: (type: string, startDate?: string, endDate?: string) => 
      axiosInstance.get<Report>('/api/admin/reports', { params: { type, startDate, endDate } }),
    getDashboardStats: () => axiosInstance.get<DashboardStats>('/api/admin/dashboard')
  }
};