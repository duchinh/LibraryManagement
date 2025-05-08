import React from "react";
import { BookBorrowingRequestResponseDto } from "../types/borrow.types";
import { formatDate } from "../helpers/dateHelpers";

interface RequestDetailsToastProps {
  request: BookBorrowingRequestResponseDto;
  getStatusText: (status: number) => string;
}

const BorrowDetailsToast: React.FC<RequestDetailsToastProps> = ({
  request,
  getStatusText,
}) => (
  <div className="bg-gradient-to-r from-indigo-500 via-indigo-600 to-indigo-700 p-6 rounded-lg shadow-xl max-w-xs text-white">
    <div className="font-semibold text-xl mb-2">Request Details</div>
    <div className="space-y-2">
      <div>
        <strong>Books:</strong> {request.bookTitles.join(", ")}
      </div>
      <div>
        <strong>Status:</strong> {getStatusText(request.status)}
      </div>
      {request.status === 1 && (
        <div className="flex flex-col space-y-2">
          <strong>Approve Note:</strong> {request.note}
          <strong>Approve Date:</strong> {formatDate(request.approvedDate)}
        </div>
      )}
      {request.status === 2 && (
        <div className="flex flex-col space-y-2">
          <strong>Reject Note:</strong> {request.note}
          <strong>Returned Date:</strong> {formatDate(request.returnedDate)}
        </div>
      )}
    </div>
  </div>
);

export default BorrowDetailsToast;
