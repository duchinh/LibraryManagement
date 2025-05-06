import { useState, useEffect } from 'react';
import { api } from '../../constants/api';
import { handleApiError } from '../../utils/errorHandler';
import { BorrowRequest } from '../../interfaces/borrow.interface';
import { message } from 'antd';

export default function MyBorrowedBooks() {
  const [requests, setRequests] = useState<BorrowRequest[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchMyRequests();
  }, []);

  const fetchMyRequests = async () => {
    try {
      const response = await api.borrowRequests.getMyRequests();
      setRequests(response.data);
    } catch (error) {
      handleApiError(error);
    } finally {
      setLoading(false);
    }
  };

  const handleReturnBooks = async (id: number) => {
    try {
      await api.borrowRequests.returnBook(id);
      message.success('Đã trả sách thành công');
      fetchMyRequests();
    } catch (error) {
      handleApiError(error);
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
        <h1 className="text-2xl font-semibold text-gray-900 mb-6">Sách đã mượn</h1>
        
        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <ul className="divide-y divide-gray-200">
            {requests.map((request) => (
              <li key={request.id}>
                <div className="px-4 py-4 sm:px-6">
                  <div className="flex items-center justify-between">
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-blue-600 truncate">
                        Ngày mượn: {new Date(request.borrowDate).toLocaleDateString()}
                      </p>
                      <p className="mt-1 text-sm text-gray-500">
                        Ngày trả: {new Date(request.dueDate).toLocaleDateString()}
                      </p>
                      <p className="mt-1 text-sm text-gray-500">
                        Trạng thái: {request.status}
                      </p>
                      <div className="mt-2">
                        <h4 className="text-sm font-medium text-gray-900">Danh sách sách:</h4>
                        <ul className="mt-1 text-sm text-gray-500">
                          {request.details.map((detail) => (
                            <li key={detail.id} className="flex items-center justify-between">
                              <span>{detail.bookTitle}</span>
                              {request.status === 'Approved' && (
                                <button
                                  onClick={() => handleReturnBooks(request.id)}
                                  className="ml-4 inline-flex items-center px-2.5 py-1.5 border border-transparent text-xs font-medium rounded text-blue-700 bg-blue-100 hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
                                >
                                  Trả sách
                                </button>
                              )}
                            </li>
                          ))}
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
              </li>
            ))}
            {requests.length === 0 && (
              <li className="px-4 py-4 sm:px-6 text-center text-gray-500">
                Bạn chưa mượn sách nào
              </li>
            )}
          </ul>
        </div>
      </div>
    </div>
  );
} 