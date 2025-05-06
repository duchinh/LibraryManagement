import { useState, useEffect } from 'react';
import { api } from '../../constants/api';
import { BorrowRequest, BorrowRequestUpdateDTO } from '../../interfaces/borrow.interface';
import { handleApiError } from '../../utils/errorHandler';
import { message } from 'antd';

export default function BorrowRequests() {
  const [requests, setRequests] = useState<BorrowRequest[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedStatus, setSelectedStatus] = useState<string | null>(null);

  useEffect(() => {
    fetchRequests();
  }, []);

  const fetchRequests = async () => {
    try {
      const response = await api.borrowRequests.getAll();
      setRequests(response.data);
    } catch (error) {
      handleApiError(error);
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (id: number) => {
    try {
      const updateDTO: BorrowRequestUpdateDTO = {
        status: 'Approved'
      };
      await api.borrowRequests.update(id, updateDTO);
      message.success('Đã duyệt yêu cầu mượn sách');
      fetchRequests();
    } catch (error) {
      handleApiError(error);
    }
  };

  const handleReject = async (id: number) => {
    try {
      const updateDTO: BorrowRequestUpdateDTO = {
        status: 'Rejected'
      };
      await api.borrowRequests.update(id, updateDTO);
      message.success('Đã từ chối yêu cầu mượn sách');
      fetchRequests();
    } catch (error) {
      handleApiError(error);
    }
  };

  const filteredRequests = selectedStatus
    ? requests.filter(request => request.status === selectedStatus)
    : requests;

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
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-semibold text-gray-900">Yêu cầu mượn sách</h1>
          <div className="flex space-x-4">
            <button
              onClick={() => setSelectedStatus(null)}
              className={`px-4 py-2 rounded-md ${
                selectedStatus === null
                  ? 'bg-blue-600 text-white'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Tất cả
            </button>
            <button
              onClick={() => setSelectedStatus('Pending')}
              className={`px-4 py-2 rounded-md ${
                selectedStatus === 'Pending'
                  ? 'bg-blue-600 text-white'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Đang chờ
            </button>
            <button
              onClick={() => setSelectedStatus('Approved')}
              className={`px-4 py-2 rounded-md ${
                selectedStatus === 'Approved'
                  ? 'bg-blue-600 text-white'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Đã duyệt
            </button>
            <button
              onClick={() => setSelectedStatus('Rejected')}
              className={`px-4 py-2 rounded-md ${
                selectedStatus === 'Rejected'
                  ? 'bg-blue-600 text-white'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Đã từ chối
            </button>
          </div>
        </div>

        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <ul className="divide-y divide-gray-200">
            {filteredRequests.map((request) => (
              <li key={request.id}>
                <div className="px-4 py-4 sm:px-6">
                  <div className="flex items-center justify-between">
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-blue-600 truncate">
                        Người yêu cầu: {request.requestorName}
                      </p>
                      <p className="mt-1 text-sm text-gray-500">
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
                    {request.status === 'Pending' && (
                      <div className="flex space-x-2">
                        <button
                          onClick={() => handleApprove(request.id)}
                          className="inline-flex items-center px-3 py-1 border border-transparent text-sm font-medium rounded-md text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
                        >
                          Duyệt
                        </button>
                        <button
                          onClick={() => handleReject(request.id)}
                          className="inline-flex items-center px-3 py-1 border border-transparent text-sm font-medium rounded-md text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
                        >
                          Từ chối
                        </button>
                      </div>
                    )}
                  </div>
                </div>
              </li>
            ))}
            {filteredRequests.length === 0 && (
              <li className="px-4 py-4 sm:px-6 text-center text-gray-500">
                Không có yêu cầu mượn sách nào
              </li>
            )}
          </ul>
        </div>
      </div>
    </div>
  );
} 