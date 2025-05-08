import React from "react";
import { BookDTO } from "../../interfaces/book.interface";
import { Edit, Trash } from "lucide-react";

interface BookTableProps {
  books: BookDTO[];
  onEdit: (book: BookDTO) => void;
  onDelete: (book: BookDTO) => void;
}

const BookManageTable: React.FC<BookTableProps> = ({
  books,
  onEdit,
  onDelete,
}) => {
  const sortedBooks = [...books].sort((a, b) => {
    const dateA = new Date(a.createdAt).getTime();
    const dateB = new Date(b.createdAt).getTime();
    return dateB - dateA;
  });

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
        <thead className="bg-indigo-400">
          <tr>
            {[
              "Title",
              "Author",
              "Category",
              "Quantity",
              "Available",
              "Actions",
            ].map((h) => (
              <th
                key={h}
                className="text-left px-4 py-3 text-sm font-bold text-white"
              >
                {h}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200">
          {sortedBooks.length > 0 ? (
            sortedBooks.map((book) => (
              <tr
                key={book.id}
                className="odd:bg-white even:bg-gray-50 hover:bg-gray-100 transition"
              >
                <td className="px-4 py-3 text-sm text-gray-900">
                  {book.title}
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  {book.author}
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  <span className="inline-block px-2 py-1 rounded font-semibold bg-gray-300 text-gray-700 w-40 text-center leading-6 mb-1">
                    {book.categoryName ?? "N/A"}
                  </span>
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  {book.totalCopies}
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  <span
                    className={`px-2 py-1 rounded font-semibold ${
                      book.availableCopies === 0 ? "bg-red-200" : "bg-green-200"
                    }`}
                  >
                    {book.availableCopies} available
                  </span>
                </td>
                <td className="px-4 py-3 flex gap-3 items-center">
                  <button
                    onClick={() => onEdit(book)}
                    title="Edit"
                    className="text-indigo-500 cursor-pointer hover:text-indigo-700 w-5 h-5 transition"
                  >
                    <Edit />
                  </button>
                  <button
                    onClick={() => onDelete(book)}
                    title="Delete"
                    className="text-red-500 cursor-pointer hover:text-red-700 w-5 h-5 transition"
                  >
                    <Trash />
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td
                colSpan={6}
                className="text-center py-6 text-gray-500 text-sm"
              >
                No books found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default BookManageTable;
