import React, { useState, useEffect } from 'react';
import { api } from '../../constants/api';
import { DashboardStats } from '../../interfaces';
import { Book } from '../../interfaces/book.interface';
import { BorrowRequest } from '../../interfaces/borrow.interface';
import { User } from '../../interfaces/user.interface';
import { message } from 'antd';

export default function AdminDashboard() {
  const [stats, setStats] = useState<DashboardStats>({
    totalBooks: 0,
    totalUsers: 0,
    pendingRequests: 0,
    overdueBooks: 0
  });
  const [recentBooks, setRecentBooks] = useState<Book[]>([]);
  const [recentRequests, setRecentRequests] = useState<BorrowRequest[]>([]);
  const [recentUsers, setRecentUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    try {
      setLoading(true);
      const [dashboardStats, books, borrowRequests, users] = await Promise.all([
        api.admin.getDashboardStats(),
        api.books.getAll(),
        api.borrowRequests.getAll(),
        api.users.getAll()
      ]);

      setStats(dashboardStats.data);

      // Lấy dữ liệu gần đây
      setRecentBooks(books.data.slice(0, 5));
      setRecentRequests(borrowRequests.data.slice(0, 5));
      setRecentUsers(users.data.slice(0, 5));
    } catch (error) {
      console.error('Error fetching dashboard data:', error);
      message.error('Không thể tải dữ liệu bảng điều khiển');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-8">Bảng điều khiển</h1>

      {/* Thống kê */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-700 mb-2">Sách</h3>
          <div className="flex justify-between">
            <div>
              <p className="text-3xl font-bold text-blue-600">{stats.totalBooks}</p>
              <p className="text-sm text-gray-500">Tổng số sách</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-700 mb-2">Người dùng</h3>
          <div className="flex justify-between">
            <div>
              <p className="text-3xl font-bold text-blue-600">{stats.totalUsers}</p>
              <p className="text-sm text-gray-500">Tổng số người dùng</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-700 mb-2">Yêu cầu đang chờ</h3>
          <div className="flex justify-between">
            <div>
              <p className="text-3xl font-bold text-yellow-600">{stats.pendingRequests}</p>
              <p className="text-sm text-gray-500">Yêu cầu chờ duyệt</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-700 mb-2">Sách quá hạn</h3>
          <div className="flex justify-between">
            <div>
              <p className="text-3xl font-bold text-red-600">{stats.overdueBooks}</p>
              <p className="text-sm text-gray-500">Sách chưa trả</p>
            </div>
          </div>
        </div>
      </div>

      {/* Dữ liệu gần đây */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Sách gần đây */}
        <div className="bg-white rounded-lg shadow">
          <div className="p-6">
            <h3 className="text-lg font-semibold text-gray-700 mb-4">Sách gần đây</h3>
            <div className="space-y-4">
              {recentBooks.length > 0 ? (
                recentBooks.map(book => (
                  <div key={book.id} className="flex items-center space-x-4">
                    <div className="flex-1">
                      <p className="font-medium">{book.title}</p>
                      <p className="text-sm text-gray-500">{book.author}</p>
                    </div>
                    <span className={`px-2 py-1 text-xs rounded-full ${
                      book.status === 'Available' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                    }`}>
                      {book.status}
                    </span>
                  </div>
                ))
              ) : (
                <p className="text-gray-500">Không có sách nào</p>
              )}
            </div>
          </div>
        </div>

        {/* Yêu cầu gần đây */}
        <div className="bg-white rounded-lg shadow">
          <div className="p-6">
            <h3 className="text-lg font-semibold text-gray-700 mb-4">Yêu cầu gần đây</h3>
            <div className="space-y-4">
              {recentRequests.length > 0 ? (
                recentRequests.map(request => (
                  <div key={request.id} className="flex items-center space-x-4">
                    <div className="flex-1">
                      <p className="font-medium">{request.requestorName}</p>
                      <p className="text-sm text-gray-500">{request.bookCount} sách</p>
                    </div>
                    <span className={`px-2 py-1 text-xs rounded-full ${
                      request.status === 'Pending' ? 'bg-yellow-100 text-yellow-800' :
                      request.status === 'Approved' ? 'bg-green-100 text-green-800' :
                      'bg-red-100 text-red-800'
                    }`}>
                      {request.status}
                    </span>
                  </div>
                ))
              ) : (
                <p className="text-gray-500">Không có yêu cầu nào</p>
              )}
            </div>
          </div>
        </div>

        {/* Người dùng gần đây */}
        <div className="bg-white rounded-lg shadow">
          <div className="p-6">
            <h3 className="text-lg font-semibold text-gray-700 mb-4">Người dùng gần đây</h3>
            <div className="space-y-4">
              {recentUsers.length > 0 ? (
                recentUsers.map(user => (
                  <div key={user.id} className="flex items-center space-x-4">
                    <div className="flex-1">
                      <p className="font-medium">{user.fullName}</p>
                      <p className="text-sm text-gray-500">{user.email}</p>
                    </div>
                    <span className={`px-2 py-1 text-xs rounded-full ${
                      user.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                    }`}>
                      {user.isActive ? 'Hoạt động' : 'Không hoạt động'}
                    </span>
                  </div>
                ))
              ) : (
                <p className="text-gray-500">Không có người dùng nào</p>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
} 