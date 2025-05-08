import React from "react";

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  totalRecords: number;
  onPageChange: (page: number) => void;
  onPrevious: () => void;
  onNext: () => void;
  onItemsPerPageChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  itemsPerPage: number;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onPageChange,
  onPrevious,
  onNext,
  onItemsPerPageChange,
  itemsPerPage,
}) => {
  const handleItemsPerPageChange = (
    e: React.ChangeEvent<HTMLSelectElement>
  ) => {
    onPageChange(1);
    onItemsPerPageChange(e);
  };

  return (
    <div className="flex justify-between items-center mb-4 pt-6">
      <div className="flex items-center">
        <label htmlFor="itemsPerPage" className="mr-2 text-sm font-medium">
          Show:
        </label>
        <select
          id="itemsPerPage"
          value={itemsPerPage}
          onChange={handleItemsPerPageChange}
          className="border border-gray-300 rounded px-2 py-1 text-sm"
        >
          {[5, 10, 20, 50].map((count) => (
            <option key={count} value={count}>
              {count}
            </option>
          ))}
        </select>
        <p className="pl-3 text-sm font-medium">records per page</p>
      </div>

      <div className="flex justify-center mt-6">
        <nav className="flex items-center gap-4 text-sm">
          <button
            onClick={onPrevious}
            disabled={currentPage === 1}
            className={`px-3 py-1  ${
              currentPage === 1
                ? "text-gray-400 cursor-not-allowed"
                : "text-gray-700 cursor-pointer hover:text-purple-600"
            }`}
          >
            Previous
          </button>

          {Array.from({ length: totalPages }, (_, index) => index + 1).map(
            (page) => (
              <button
                key={page}
                onClick={() => onPageChange(page)}
                className={`px-3 py-1 border-b-2 cursor-pointer ${
                  currentPage === page
                    ? "text-purple-600 border-purple-600"
                    : "text-gray-700 border-transparent hover:border-gray-300"
                }`}
              >
                {page}
              </button>
            )
          )}

          <button
            onClick={onNext}
            disabled={currentPage === totalPages}
            className={`px-3 py-1  ${
              currentPage === totalPages
                ? "text-gray-400 cursor-not-allowed"
                : "text-gray-700 cursor-pointer hover:text-purple-600"
            }`}
          >
            Next
          </button>
        </nav>
      </div>
    </div>
  );
};

export default Pagination;
