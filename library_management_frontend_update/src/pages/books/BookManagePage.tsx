import React, { useEffect, useState } from "react";
import { getAllBooks, deleteBook } from "../../services/bookService";
import { BookDTO } from "../../interfaces/book.interface";
import CreateBookForm from "../../components/form/CreateBookForm";
import UpdateBookForm from "../../components/form/UpdateBookForm";
import { Plus } from "lucide-react";
import Pagination from "../../components/Pagination";
import BookManageTable from "../../components/table/BookManageTable";
import Modal from "../../components/Modal";

const BookManagePage: React.FC = () => {
  const [books, setBooks] = useState<BookDTO[]>([]);
  const [modalData, setModalData] = useState<{
    type: "create" | "edit" | "delete" | null;
    book: BookDTO | null;
  }>({ type: null, book: null });

  const [pagination, setPagination] = useState({
    currentPage: 1,
    itemsPerPage: 10,
  });

  const loadBooks = async () => {
    const data = await getAllBooks();
    setBooks(data);
  };

  useEffect(() => {
    loadBooks();
  }, []);

  const handlePaginationChange = (
    field: "currentPage" | "itemsPerPage",
    value: number
  ) => {
    if (field === "itemsPerPage") {
      setPagination((prev) => ({
        ...prev,
        itemsPerPage: value,
        currentPage: 1,
      }));
    } else {
      setPagination((prev) => ({
        ...prev,
        currentPage: value,
      }));
    }
  };

  const handleDelete = async () => {
    if (modalData.book) {
      await deleteBook(modalData.book.id);
      setModalData({ type: null, book: null });
      loadBooks();
    }
  };

  const handleCloseModal = () => {
    setModalData({ type: null, book: null });
  };

  const paginatedBooks = books.slice(
    (pagination.currentPage - 1) * pagination.itemsPerPage,
    pagination.currentPage * pagination.itemsPerPage
  );

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Books</h1>
        <button
          onClick={() => setModalData({ type: "create", book: null })}
          className="flex items-center gap-2 bg-indigo-400 text-white px-4 py-2 rounded hover:bg-indigo-600 cursor-pointer transition"
        >
          <Plus className="w-5 h-5" />
          Create Book
        </button>
      </div>

      <BookManageTable
        books={paginatedBooks}
        onEdit={(book) => setModalData({ type: "edit", book })}
        onDelete={(book) => setModalData({ type: "delete", book })}
      />

      <Pagination
        currentPage={pagination.currentPage}
        itemsPerPage={pagination.itemsPerPage}
        totalRecords={books.length}
        totalPages={Math.ceil(books.length / pagination.itemsPerPage)}
        onPageChange={(page) => handlePaginationChange("currentPage", page)}
        onItemsPerPageChange={(event) =>
          handlePaginationChange("itemsPerPage", parseInt(event.target.value))
        }
        onPrevious={() =>
          handlePaginationChange("currentPage", pagination.currentPage - 1)
        }
        onNext={() =>
          handlePaginationChange("currentPage", pagination.currentPage + 1)
        }
      />

      <Modal
        isOpen={modalData.type !== null}
        onClose={handleCloseModal}
        title={
          modalData.type === "create"
            ? "Create Book"
            : modalData.type === "edit"
            ? "Edit Book"
            : "Confirm Deletion"
        }
      >
        {modalData.type === "create" && (
          <CreateBookForm onCreated={loadBooks} onCancel={handleCloseModal} />
        )}
        {modalData.type === "edit" && modalData.book && (
          <UpdateBookForm
            book={modalData.book}
            onUpdated={loadBooks}
            onCancel={handleCloseModal}
          />
        )}
        {modalData.type === "delete" && modalData.book && (
          <div>
            <p>
              Are you sure you want to delete{" "}
              <strong>{modalData.book.title}</strong>?
            </p>
            <div className="flex justify-end gap-4 mt-4">
              <button
                onClick={handleCloseModal}
                className="bg-gray-200 px-4 py-2 rounded hover:bg-gray-300"
              >
                Cancel
              </button>
              <button
                onClick={handleDelete}
                className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
              >
                Delete
              </button>
            </div>
          </div>
        )}
      </Modal>
    </div>
  );
};

export default BookManagePage;
