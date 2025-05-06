import { message } from 'antd';
import { Book } from '../types/book';

export const validateUsername = (username: string): boolean => {
  if (!username) {
    message.error('Tên đăng nhập không được để trống');
    return false;
  }
  if (username.length < 3) {
    message.error('Tên đăng nhập phải có ít nhất 3 ký tự');
    return false;
  }
  if (username.length > 50) {
    message.error('Tên đăng nhập không được vượt quá 50 ký tự');
    return false;
  }
  if (!/^[a-zA-Z0-9_]+$/.test(username)) {
    message.error('Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới');
    return false;
  }
  return true;
};

export const validatePassword = (password: string): boolean => {
  if (!password) {
    message.error('Mật khẩu không được để trống');
    return false;
  }
  if (password.length < 6) {
    message.error('Mật khẩu phải có ít nhất 6 ký tự');
    return false;
  }
  if (!/[A-Z]/.test(password)) {
    message.error('Mật khẩu phải chứa ít nhất một chữ hoa');
    return false;
  }
  if (!/[a-z]/.test(password)) {
    message.error('Mật khẩu phải chứa ít nhất một chữ thường');
    return false;
  }
  if (!/[0-9]/.test(password)) {
    message.error('Mật khẩu phải chứa ít nhất một số');
    return false;
  }
  if (!/[^a-zA-Z0-9]/.test(password)) {
    message.error('Mật khẩu phải chứa ít nhất một ký tự đặc biệt');
    return false;
  }
  return true;
};

export const validateEmail = (email: string): boolean => {
  if (!email) {
    message.error('Email không được để trống');
    return false;
  }
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
    message.error('Email không hợp lệ');
    return false;
  }
  if (email.length > 100) {
    message.error('Email không được vượt quá 100 ký tự');
    return false;
  }
  return true;
};

export const validateFullName = (fullName: string): boolean => {
  if (!fullName) {
    message.error('Họ và tên không được để trống');
    return false;
  }
  if (fullName.length > 100) {
    message.error('Họ và tên không được vượt quá 100 ký tự');
    return false;
  }
  return true;
};

export const validatePhone = (phone: string): boolean => {
  if (phone && !/^[0-9\-+()]*$/.test(phone)) {
    message.error('Số điện thoại không hợp lệ');
    return false;
  }
  if (phone && phone.length > 20) {
    message.error('Số điện thoại không được vượt quá 20 ký tự');
    return false;
  }
  return true;
};

export const validateISBN = (isbn: string): boolean => {
  if (!isbn) {
    message.error('ISBN không được để trống');
    return false;
  }
  if (!/^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$/.test(isbn)) {
    message.error('ISBN không hợp lệ');
    return false;
  }
  return true;
};

export const validateBookTitle = (title: string): boolean => {
  if (!title) {
    message.error('Tên sách không được để trống');
    return false;
  }
  if (title.length > 200) {
    message.error('Tên sách không được vượt quá 200 ký tự');
    return false;
  }
  return true;
};

export const validateAuthor = (author: string): boolean => {
  if (!author) {
    message.error('Tác giả không được để trống');
    return false;
  }
  if (author.length > 100) {
    message.error('Tên tác giả không được vượt quá 100 ký tự');
    return false;
  }
  return true;
};

export const validatePublisher = (publisher: string): boolean => {
  if (!publisher) {
    message.error('Nhà xuất bản không được để trống');
    return false;
  }
  if (publisher.length > 100) {
    message.error('Tên nhà xuất bản không được vượt quá 100 ký tự');
    return false;
  }
  return true;
};

export const validatePublicationYear = (year: number): boolean => {
  if (!year) {
    message.error('Năm xuất bản không được để trống');
    return false;
  }
  const currentYear = new Date().getFullYear();
  if (year < 1800 || year > currentYear) {
    message.error(`Năm xuất bản phải từ 1800 đến ${currentYear}`);
    return false;
  }
  return true;
};

export const validateQuantity = (quantity: number): boolean => {
  if (!quantity) {
    message.error('Số lượng không được để trống');
    return false;
  }
  if (quantity <= 0) {
    message.error('Số lượng phải lớn hơn 0');
    return false;
  }
  return true;
};

export const validateDescription = (description: string): boolean => {
  if (description && description.length > 2000) {
    message.error('Mô tả không được vượt quá 2000 ký tự');
    return false;
  }
  return true;
};

export const validateBookIds = (bookIds: number[]): boolean => {
  if (!bookIds || bookIds.length === 0) {
    message.error('Danh sách sách không được để trống');
    return false;
  }
  if (bookIds.length > 5) {
    message.error('Không được mượn quá 5 cuốn sách cùng lúc');
    return false;
  }
  if (new Set(bookIds).size !== bookIds.length) {
    message.error('Không được chọn trùng sách');
    return false;
  }
  return true;
};

export const validateRejectReason = (reason: string): boolean => {
  if (!reason) {
    message.error('Lý do từ chối không được để trống');
    return false;
  }
  if (reason.length > 500) {
    message.error('Lý do từ chối không được vượt quá 500 ký tự');
    return false;
  }
  return true;
};

export const validateOverdueBooks = (books: Book[]): boolean => {
  const now = new Date();
  const overdueBooks = books.filter(book => 
    book.dueDate && new Date(book.dueDate) < now
  );
  if (overdueBooks.length > 0) {
    message.error('Bạn có sách đã quá hạn, vui lòng trả sách trước khi mượn sách mới');
    return false;
  }
  return true;
};

export const validateMonthlyBorrowing = (count: number): boolean => {
  if (count >= 3) {
    message.error('Bạn đã mượn 3 lần trong tháng này');
    return false;
  }
  return true;
};

export const validateBorrowingRequest = async (
  bookIds: number[],
  currentBorrowings: Book[],
  monthlyBorrowingCount: number
): Promise<boolean> => {
  // Kiểm tra số lượng sách
  if (!validateBookIds(bookIds)) {
    return false;
  }

  // Kiểm tra sách quá hạn
  if (!validateOverdueBooks(currentBorrowings)) {
    return false;
  }

  // Kiểm tra số lần mượn trong tháng
  if (!validateMonthlyBorrowing(monthlyBorrowingCount)) {
    return false;
  }

  return true;
}; 