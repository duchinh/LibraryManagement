export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  categoryId: number;
  categoryName: string;
  quantity: number;
  availableQuantity: number;
  status: string;
  dueDate?: string; // Ngày hết hạn mượn sách
} 