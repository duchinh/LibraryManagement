import { AxiosError } from 'axios';
import { toast } from 'react-toastify';

interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
}

export const handleApiError = (error: unknown) => {
  if (error instanceof AxiosError) {
    const apiError = error.response?.data as ApiError;
    
    if (apiError?.errors) {
      // Hiển thị tất cả các lỗi validation
      Object.values(apiError.errors).forEach(errors => {
        errors.forEach(errorMessage => {
          toast.error(errorMessage);
        });
      });
    } else if (apiError?.message) {
      // Hiển thị thông báo lỗi chung
      toast.error(apiError.message);
    } else {
      // Hiển thị thông báo lỗi mặc định
      toast.error('Đã xảy ra lỗi. Vui lòng thử lại sau.');
    }
  } else {
    // Xử lý các lỗi không phải từ API
    toast.error('Đã xảy ra lỗi không xác định. Vui lòng thử lại sau.');
  }
}; 