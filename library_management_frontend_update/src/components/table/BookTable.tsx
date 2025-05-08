import React from "react";
import { BookDTO } from "../../interfaces/book.interface";

interface BookTableProps {
  books: BookDTO[];
  selectedBookIds: string[];
  toggleSelectBook: (bookId: string) => void;
}

const BookTable: React.FC<BookTableProps> = ({
  books,
  selectedBookIds,
  toggleSelectBook,
}) => {
  return (
    <div className="overflow-x-auto mb-10">
      <table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
        <thead className="bg-indigo-400">
          <tr>
            <th className="px-4 py-3 text-left border-r border-indigo-500 last:border-r-0 text-white">
              Select
            </th>
            <th className="px-4 py-3 text-left border-r border-indigo-500 last:border-r-0 text-white">
              Title
            </th>
            <th className="px-4 py-3 text-left border-r border-indigo-500 last:border-r-0 text-white">
              Author
            </th>
            <th className="px-4 py-3 text-left border-r border-indigo-500 last:border-r-0 text-white">
              Category
            </th>
            <th className="px-4 py-3 text-left border-r border-indigo-500 last:border-r-0 text-white">
              Available
            </th>
          </tr>
        </thead>
        <tbody>
          {books.map((book) => (
            <tr
              key={book.id}
              className="odd:bg-white even:bg-gray-50 hover:bg-gray-100"
            >
              <td className="px-4 py-3 border-r border-gray-200 last:border-r-0">
                <input
                  type="checkbox"
                  className="h-5 w-5 "
                  checked={selectedBookIds.includes(book.id)}
                  onChange={() => toggleSelectBook(book.id)}
                  disabled={
                    !selectedBookIds.includes(book.id) &&
                    selectedBookIds.length >= 5
                  }
                />
              </td>
              <td className="px-4 py-3 border-r border-gray-200 last:border-r-0">
                {book.title}
              </td>
              <td className="px-4 py-3 border-r border-gray-200 last:border-r-0">
                {book.author}
              </td>
              <td className="px-4 py-3 border-r border-gray-200 last:border-r-0">
                <span className="inline-block px-2 py-1 rounded font-semibold bg-gray-300 text-gray-700 w-40 text-center leading-6 mb-1">
                  {book.categoryName ?? "N/A"}
                </span>
              </td>
              <td className="px-4 py-3 border-r border-gray-200 last:border-r-0">
                <span
                  className={`inline-block px-2 py-1 rounded font-semibold ${
                    book.availableCopies === 0 ? "bg-red-200" : "bg-green-200"
                  }`}
                >
                  {book.availableCopies} available
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default BookTable;
