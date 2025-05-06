import axios from 'axios';
import { API_URL } from '../constants/api';
import { message } from 'antd';

// Tạo instance axios với cấu hình mặc định
const axiosInstance = axios.create({
  baseURL: API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Biến để theo dõi số lượng request đang chạy
let activeRequests = 0;

// Request interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    // Lấy token từ localStorage
    const token = localStorage.getItem('token');
    
    // Nếu có token thì thêm vào header
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    // Tăng số lượng request đang chạy
    activeRequests++;
    
    // Hiển thị loading nếu đây là request đầu tiên
    if (activeRequests === 1) {
      message.loading({ content: 'Đang tải...', key: 'loading' });
    }
    
    return config;
  },
  (error) => {
    // Giảm số lượng request đang chạy
    activeRequests--;
    
    // Ẩn loading nếu không còn request nào
    if (activeRequests === 0) {
      message.destroy('loading');
    }
    
    return Promise.reject(error);
  }
);

// Response interceptor
axiosInstance.interceptors.response.use(
  (response) => {
    // Giảm số lượng request đang chạy
    activeRequests--;
    
    // Ẩn loading nếu không còn request nào
    if (activeRequests === 0) {
      message.destroy('loading');
    }
    
    return response;
  },
  async (error) => {
    // Giảm số lượng request đang chạy
    activeRequests--;
    
    // Ẩn loading nếu không còn request nào
    if (activeRequests === 0) {
      message.destroy('loading');
    }

    const originalRequest = error.config;

    // Nếu lỗi 401 (Unauthorized) và chưa thử refresh token
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Lấy refresh token từ localStorage
        const refreshToken = localStorage.getItem('refreshToken');
        
        if (!refreshToken) {
          // Nếu không có refresh token thì logout
          localStorage.removeItem('token');
          localStorage.removeItem('refreshToken');
          localStorage.removeItem('user');
          message.error('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
          window.location.href = '/login';
          return Promise.reject(error);
        }

        // Gọi API refresh token
        const response = await axios.post(`${API_URL}/auth/refresh`, { refreshToken });
        const { accessToken, refreshToken: newRefreshToken } = response.data.data;

        // Lưu token mới
        localStorage.setItem('token', accessToken);
        localStorage.setItem('refreshToken', newRefreshToken);

        // Thêm token mới vào header và thử lại request
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return axiosInstance(originalRequest);
      } catch (refreshError) {
        // Nếu refresh token thất bại thì logout
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
        message.error('Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }

    // Xử lý các lỗi khác
    if (error.response) {
      // Lỗi từ server
      const { status, data } = error.response;
      
      switch (status) {
        case 400:
          message.error(data.message || 'Yêu cầu không hợp lệ');
          break;
        case 403:
          message.error(data.message || 'Bạn không có quyền thực hiện thao tác này');
          window.location.href = '/unauthorized';
          break;
        case 404:
          message.error(data.message || 'Không tìm thấy tài nguyên');
          break;
        case 500:
          message.error(data.message || 'Lỗi máy chủ');
          break;
        default:
          message.error(data.message || 'Đã xảy ra lỗi');
      }
    } else if (error.request) {
      // Không nhận được response từ server
      message.error('Không thể kết nối đến máy chủ');
    } else {
      // Lỗi khi setup request
      message.error('Lỗi khi gửi yêu cầu');
    }

    // Thử lại request nếu chưa vượt quá số lần thử lại
    if (originalRequest && !originalRequest._retryCount) {
      originalRequest._retryCount = 1;
      return axiosInstance(originalRequest);
    }

    return Promise.reject(error);
  }
);

export default axiosInstance; 