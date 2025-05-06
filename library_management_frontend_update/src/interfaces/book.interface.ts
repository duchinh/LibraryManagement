export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  category: string;
  description?: string;
  status: 'Available' | 'Borrowed' | 'Reserved';
  quantity: number;
  availableQuantity: number;
  publishedYear?: number;
  publisher?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface Category {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
  updatedAt: string;
}

export interface BookCreateDTO {
  title: string;
  author: string;
  isbn: string;
  category: string;
  description?: string;
  quantity: number;
  publishedYear?: number;
  publisher?: string;
}

export interface BookUpdateDTO {
  title?: string;
  author?: string;
  isbn?: string;
  category?: string;
  description?: string;
  quantity?: number;
  status?: 'Available' | 'Borrowed' | 'Reserved';
  publishedYear?: number;
  publisher?: string;
} 