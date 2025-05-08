export interface User {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  phoneNumber?: string;
  role: 'Admin' | 'NormalUser';
  isActive: boolean;
  lastLoginDate?: string;
  createdAt: string;
  updatedAt?: string;
  isDeleted: boolean;
}

export interface UserCreateDTO {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber?: string;
  role: 'Admin' | 'NormalUser';
}

export interface UserUpdateDTO {
  username?: string;
  email?: string;
  password?: string;
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  role?: 'Admin' | 'NormalUser';
} 