import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { api } from '../../constants/api';
import { handleApiError } from '../../utils/errorHandler';
import { Book } from '../../interfaces/book.interface';
import { BorrowRequest } from '../../interfaces/borrow.interface';

interface BorrowingFormProps {
  books: Book[];
}

export default function BorrowingForm({ books }: BorrowingFormProps) {
  const [selectedBooks, setSelectedBooks] = useState<number[]>([]);
  const [notes, setNotes] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const borrowDate = new Date();
      const dueDate = new Date();
      dueDate.setDate(dueDate.getDate() + 14); // Mượn sách trong 14 ngày

      const request: Omit<BorrowRequest, 'id' | 'createdAt' | 'updatedAt'> = {
        bookIds: selectedBooks,
        borrowDate: borrowDate.toISOString(),
        dueDate: dueDate.toISOString(),
        notes,
        status: 'Pending',
        requestorId: '', // Sẽ được set bởi backend
        requestorName: '', // Sẽ được set bởi backend
        requestDate: new Date().toISOString(),
        bookCount: selectedBooks.length,
        details: []
      };
      await api.borrowRequests.create(request);
      navigate('/borrow-requests');
    } catch (error) {
      handleApiError(error);
    }
  };

  const handleBookSelect = (bookId: number) => {
    setSelectedBooks(prev => {
      if (prev.includes(bookId)) {
        return prev.filter(id => id !== bookId);
      }
      return [...prev, bookId];
    });
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {books.map(book => (
          <div
            key={book.id}
            className={`p-4 border rounded-lg cursor-pointer ${
              selectedBooks.includes(Number(book.id)) ? 'border-blue-500 bg-blue-50' : 'border-gray-200'
            }`}
            onClick={() => handleBookSelect(Number(book.id))}
          >
            <h3 className="font-semibold">{book.title}</h3>
            <p className="text-sm text-gray-600">{book.author}</p>
            <p className="text-sm text-gray-500">ISBN: {book.isbn}</p>
          </div>
        ))}
      </div>
      <div className="mt-4">
        <label htmlFor="notes" className="block text-sm font-medium text-gray-700">
          Ghi chú
        </label>
        <textarea
          id="notes"
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring-blue-500"
          rows={3}
        />
      </div>
      <button
        type="submit"
        disabled={selectedBooks.length === 0}
        className="w-full py-2 px-4 bg-blue-500 text-white rounded-lg hover:bg-blue-600 disabled:bg-gray-300"
      >
        Mượn sách
      </button>
    </form>
  );
} 