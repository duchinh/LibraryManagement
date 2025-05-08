export interface BookDto {
  id: string;
  title: string;
  author: string;
  description: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  categoryId: string;
  categoryName: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateBookDto {
  title: string;
  author: string;
  description: string;
  publicationYear: number;
  totalCopies: number;
  categoryId: string;
}

export interface UpdateBookDto extends CreateBookDto {
  id: string;
  availableCopies: number;
}