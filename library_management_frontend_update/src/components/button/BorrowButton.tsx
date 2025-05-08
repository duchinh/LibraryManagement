import React from "react";
import { Plus, Loader } from "lucide-react";

interface BorrowButtonProps {
  loading: boolean;
  selectedCount: number;
  onClick: () => void;
}

const BorrowButton: React.FC<BorrowButtonProps> = ({
  loading,
  selectedCount,
  onClick,
}) => (
  <div className="mb-4">
    <button
      onClick={onClick}
      disabled={loading}
      className="flex items-center gap-2 bg-indigo-500 text-white px-4 py-2 rounded hover:bg-indigo-600 transition disabled:opacity-50"
    >
      {loading ? (
        <Loader className="animate-spin w-5 h-5" />
      ) : (
        <Plus className="w-5 h-5" />
      )}
      Borrow Selected Books
    </button>
    <p className="mt-2 text-sm text-gray-600">Selected {selectedCount} / 5</p>
  </div>
);

export default BorrowButton;
