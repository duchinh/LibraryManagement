import { AxiosError } from 'axios';
import { toast } from 'react-toastify';

export const handleApiError = (error: unknown) => {
  if (error instanceof AxiosError) {
    const message = error.response?.data?.message || error.message;
    toast.error(message);
  } else {
    toast.error('Đã xảy ra lỗi');
  }
}; 