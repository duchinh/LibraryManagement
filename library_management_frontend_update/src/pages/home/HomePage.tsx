import React, { useEffect, useState } from "react";
import { getAllBooks } from "../../services/bookService";
import { borrowBooks } from "../../services/borrowingService";
import { useUserContext } from "../../contexts/UserContext";
import toast from "react-hot-toast";
import BookTable from "../../components/table/BookTable";
import BorrowButton from "../../components/button/BorrowButton";
import Pagination from "../../components/Pagination";
import { BookDTO } from "../../interfaces/book.interface";
import { CreateBorrowingRequestDTO } from "../../interfaces/borrow.interface";

const HomePage: React.FC = () => {
  const [books, setBooks] = useState<BookDTO[]>([]);
  const [selectedBookIds, setSelectedBookIds] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const { user, isLoading: userLoading } = useUserContext();
  const userId = user?.id || "";

  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);

  useEffect(() => {
    if (!userLoading && userId) {
      (async () => {
        try {
          const bookData = await getAllBooks();
          setBooks(bookData);
        } catch (err) {
          setError("Failed to load books.");
        }
      })();
    }
  }, [userLoading, userId]);

  const toggleSelectBook = (bookId: string) => {
    if (selectedBookIds.includes(bookId)) {
      setSelectedBookIds(selectedBookIds.filter((id) => id !== bookId));
    } else if (selectedBookIds.length < 5) {
      setSelectedBookIds([...selectedBookIds, bookId]);
    } else {
      setError("You can only select up to 5 books per request.");
    }
  };

  const handleBorrow = async () => {
    if (selectedBookIds.length === 0) {
      toast.error("Select at least one book.");
      return;
    }

    setLoading(true);
    const dto: CreateBorrowingRequestDTO = {
      userId,
      bookIds: selectedBookIds,
      note: "Borrowing request",
    };

    try {
      await borrowBooks(dto);
      toast.success("Borrow request submitted!");
      setSelectedBookIds([]);
      setTimeout(() => {
        window.location.reload();
      }, 2000);
    } catch (e: any) {
      const errorMessage = e.response?.data?.error;

      if (errorMessage.includes("monthly request limit")) {
        toast.error("You can only borrow 3 times a month.");
      } else if (errorMessage.includes("not available")) {
        toast.error("One or more selected books are not available.");
      } else {
        toast.error(errorMessage);
      }
    } finally {
      setLoading(false);
    }
  };

  const totalRecords = books.length;
  const totalPages = Math.ceil(totalRecords / itemsPerPage);
  const paginatedBooks = books.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const handlePageChange = (page: number) => setCurrentPage(page);
  const handlePrevious = () => setCurrentPage((prev) => Math.max(prev - 1, 1));
  const handleNext = () =>
    setCurrentPage((prev) => Math.min(prev + 1, totalPages));
  const handleItemsPerPageChange = (
    e: React.ChangeEvent<HTMLSelectElement>
  ) => {
    setItemsPerPage(Number(e.target.value));
    setCurrentPage(1);
  };

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-3xl font-bold mb-6">Library</h1>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <BorrowButton
        loading={loading}
        selectedCount={selectedBookIds.length}
        onClick={handleBorrow}
      />

      <BookTable
        books={paginatedBooks}
        selectedBookIds={selectedBookIds}
        toggleSelectBook={toggleSelectBook}
      />

      <Pagination
        currentPage={currentPage}
        totalPages={totalPages}
        totalRecords={totalRecords}
        onPageChange={handlePageChange}
        onPrevious={handlePrevious}
        onNext={handleNext}
        onItemsPerPageChange={handleItemsPerPageChange}
        itemsPerPage={itemsPerPage}
      />
    </div>
  );
};

export default HomePage;
