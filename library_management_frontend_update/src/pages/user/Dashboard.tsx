import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { api } from '../../constants/api';
import { BorrowRequest } from '../../interfaces/borrow.interface';
import { message } from 'antd';

export default function UserDashboard() {
  const [requests, setRequests] = useState<BorrowRequest[]>([]);
  const [loading, setLoading] = useState(true);
  const [canBorrow, setCanBorrow] = useState(true);
  const [borrowingCount, setBorrowingCount] = useState(0);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const [requestsResponse, canBorrowResponse, borrowingCountResponse] = await Promise.all([
        api.borrowRequests.getMyRequests(),
        api.borrowRequests.canBorrowMore(),
        api.borrowRequests.getBorrowingCount()
      ]);
      setRequests(requestsResponse.data);
      setCanBorrow(canBorrowResponse.data);
      setBorrowingCount(borrowingCountResponse.data);
    } catch (error) {
      console.error('Error fetching dashboard data:', error);
      message.error('Không thể tải dữ liệu bảng điều khiển');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <div className="px-4 py-6 sm:px-0">
        <h1 className="text-2xl font-semibold text-gray-900 mb-6">Bảng điều khiển</h1>
        
        <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3">
          <div className="bg-white overflow-hidden shadow rounded-lg">
            <div className="px-4 py-5 sm:p-6">
              <dt className="text-sm font-medium text-gray-500 truncate">Sách đang mượn</dt>
              <dd className="mt-1 text-3xl font-semibold text-gray-900">{borrowingCount}</dd>
            </div>
          </div>

          <div className="bg-white overflow-hidden shadow rounded-lg">
            <div className="px-4 py-5 sm:p-6">
              <dt className="text-sm font-medium text-gray-500 truncate">Trạng thái mượn sách</dt>
              <dd className="mt-1 text-3xl font-semibold text-gray-900">
                {canBorrow ? 'Có thể mượn' : 'Không thể mượn'}
              </dd>
            </div>
          </div>

          <div className="bg-white overflow-hidden shadow rounded-lg">
            <div className="px-4 py-5 sm:p-6">
              <dt className="text-sm font-medium text-gray-500 truncate">Yêu cầu đang chờ</dt>
              <dd className="mt-1 text-3xl font-semibold text-gray-900">
                {requests.filter(r => r.status === 'Pending').length}
              </dd>
            </div>
          </div>
        </div>

        <div className="mt-8">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-medium text-gray-900">Yêu cầu mượn sách gần đây</h2>
            <Link
              to="/borrow-requests/new"
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            >
              Mượn sách mới
            </Link>
          </div>
          
          <div className="bg-white shadow overflow-hidden sm:rounded-md">
            <ul className="divide-y divide-gray-200">
              {requests.slice(0, 5).map((request) => (
                <li key={request.id}>
                  <div className="px-4 py-4 sm:px-6">
                    <div className="flex items-center justify-between">
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium text-blue-600 truncate">
                          Ngày yêu cầu: {new Date(request.requestDate).toLocaleDateString()}
                        </p>
                        <p className="mt-1 text-sm text-gray-500">
                          Số lượng sách: {request.bookCount}
                        </p>
                        <p className="mt-1 text-sm text-gray-500">
                          Trạng thái: {request.status}
                        </p>
                        {request.details.length > 0 && (
                          <div className="mt-2">
                            <p className="text-sm font-medium text-gray-700">Chi tiết sách:</p>
                            <ul className="mt-1 space-y-1">
                              {request.details.map((detail) => (
                                <li key={detail.id} className="text-sm text-gray-500">
                                  {detail.bookTitle} - Ngày mượn: {new Date(detail.borrowDate).toLocaleDateString()}
                                </li>
                              ))}
                            </ul>
                          </div>
                        )}
                      </div>
                    </div>
                  </div>
                </li>
              ))}
              {requests.length === 0 && (
                <li className="px-4 py-4 sm:px-6 text-center text-gray-500">
                  Bạn chưa có yêu cầu mượn sách nào
                </li>
              )}
            </ul>
          </div>

          {requests.length > 5 && (
            <div className="mt-4 text-center">
              <Link
                to="/borrow-requests"
                className="text-sm font-medium text-blue-600 hover:text-blue-500"
              >
                Xem tất cả yêu cầu
              </Link>
            </div>
          )}
        </div>
      </div>
    </div>
  );
} 