import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { api } from '../../constants/api';
import { handleApiError } from '../../utils/error';
import { Book } from '../../interfaces/book.interface';
import { useAuth } from '../../hooks/useAuth';

export default function BookDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchBook();
  }, [id]);

  const fetchBook = async () => {
    try {
      setLoading(true);
      const response = await api.books.getById(Number(id));
      setBook(response.data);
    } catch (error) {
      handleApiError(error);
      navigate('/books');
    } finally {
      setLoading(false);
    }
  };

  const handleBorrow = () => {
    navigate(`/books/borrow?bookId=${id}`);
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  if (!book) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">Không tìm thấy sách</h1>
          <button
            onClick={() => navigate('/books')}
            className="text-blue-600 hover:text-blue-800"
          >
            Quay lại danh sách sách
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow overflow-hidden">
        <div className="p-6">
          <div className="flex justify-between items-start mb-6">
            <div>
              <h1 className="text-2xl font-bold text-gray-900 mb-2">{book.title}</h1>
              <p className="text-gray-600">Tác giả: {book.author}</p>
            </div>
            <span className={`px-3 py-1 text-sm rounded-full ${
              book.status === 'Available' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
            }`}>
              {book.status}
            </span>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-4">Thông tin chi tiết</h2>
              <div className="space-y-2">
                <p className="text-gray-600">
                  <span className="font-medium">ISBN:</span> {book.isbn}
                </p>
                <p className="text-gray-600">
                  <span className="font-medium">Danh mục:</span> {book.category}
                </p>
                <p className="text-gray-600">
                  <span className="font-medium">Số lượng:</span> {book.availableQuantity}/{book.quantity}
                </p>
                {book.publishedYear && (
                  <p className="text-gray-600">
                    <span className="font-medium">Năm xuất bản:</span> {book.publishedYear}
                  </p>
                )}
                {book.publisher && (
                  <p className="text-gray-600">
                    <span className="font-medium">Nhà xuất bản:</span> {book.publisher}
                  </p>
                )}
              </div>
            </div>

            <div>
              <h2 className="text-lg font-semibold text-gray-900 mb-4">Mô tả</h2>
              <p className="text-gray-600">
                {book.description || 'Chưa có mô tả'}
              </p>
            </div>
          </div>

          <div className="flex justify-end space-x-4">
            <button
              onClick={() => navigate('/books')}
              className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
            >
              Quay lại
            </button>
            {user && book.status === 'Available' && (
              <button
                onClick={handleBorrow}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
              >
                Mượn sách
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
} 