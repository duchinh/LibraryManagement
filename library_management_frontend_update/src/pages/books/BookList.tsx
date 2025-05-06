import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { api } from '../../constants/api';
import { handleApiError } from '../../utils/error';
import { Book } from '../../interfaces/book.interface';

export default function BookList() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<string>('all');

  useEffect(() => {
    fetchBooks();
  }, []);

  const fetchBooks = async () => {
    try {
      setLoading(true);
      const response = await api.books.getAll();
      setBooks(response.data);
    } catch (error) {
      handleApiError(error);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    const filteredBooks = books.filter(book => {
      const matchesSearch = book.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
        book.author.toLowerCase().includes(searchQuery.toLowerCase()) ||
        book.isbn.toLowerCase().includes(searchQuery.toLowerCase());
      const matchesCategory = selectedCategory === 'all' || book.category === selectedCategory;
      return matchesSearch && matchesCategory;
    });
    setBooks(filteredBooks);
  };

  const categories = ['all', ...new Set(books.map(book => book.category))];

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-2xl font-bold">Danh sách sách</h1>
        <Link
          to="/books/borrow"
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700"
        >
          Mượn sách
        </Link>
      </div>

      {/* Tìm kiếm và lọc */}
      <div className="bg-white rounded-lg shadow p-6 mb-8">
        <form onSubmit={handleSearch} className="flex flex-col md:flex-row gap-4">
          <div className="flex-1">
            <input
              type="text"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              placeholder="Tìm kiếm theo tên sách, tác giả hoặc ISBN..."
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          <div className="w-full md:w-48">
            <select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              {categories.map(category => (
                <option key={category} value={category}>
                  {category === 'all' ? 'Tất cả danh mục' : category}
                </option>
              ))}
            </select>
          </div>
          <button
            type="submit"
            className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700"
          >
            Tìm kiếm
          </button>
        </form>
      </div>

      {/* Danh sách sách */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {books.map(book => (
          <div key={book.id} className="bg-white rounded-lg shadow overflow-hidden">
            <div className="p-6">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">{book.title}</h3>
              <p className="text-gray-600 mb-2">Tác giả: {book.author}</p>
              <p className="text-gray-600 mb-2">ISBN: {book.isbn}</p>
              <p className="text-gray-600 mb-2">Danh mục: {book.category}</p>
              <div className="flex justify-between items-center mt-4">
                <span className={`px-2 py-1 text-xs rounded-full ${
                  book.status === 'Available' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                }`}>
                  {book.status}
                </span>
                <span className="text-sm text-gray-600">
                  Còn lại: {book.availableQuantity}/{book.quantity}
                </span>
              </div>
              <Link
                to={`/books/${book.id}`}
                className="mt-4 block text-center bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700"
              >
                Chi tiết
              </Link>
            </div>
          </div>
        ))}
      </div>

      {books.length === 0 && (
        <div className="text-center py-8">
          <p className="text-gray-500">Không tìm thấy sách nào</p>
        </div>
      )}
    </div>
  );
} 