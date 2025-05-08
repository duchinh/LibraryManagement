import React from "react";
import toast from "react-hot-toast";
import { BorrowingRequestDTO } from "../../interfaces/borrow.interface";
import { formatDate } from "../../helpers/dateHelpers";
import BorrowDetailsToast from "../BorrowDetailsToast";

interface BorrowRequestsTableProps {
  requests: BorrowingRequestDTO[];
  loading: boolean;
  getStatusText: (status: number) => string;
}

const statusOptions = [
  { value: 0, label: "Pending" },
  { value: 1, label: "Approved" },
  { value: 2, label: "Rejected" },
];

const BorrowRequestsTable: React.FC<BorrowRequestsTableProps> = ({
  requests,
  loading,
  getStatusText,
}) => {
  const sortedRequests = [...requests].sort(
    (a, b) =>
      new Date(b.requestDate).getTime() - new Date(a.requestDate).getTime()
  );

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white shadow rounded-lg overflow-hidden">
        <thead className="bg-gray-200">
          <tr>
            <th className="px-4 py-3 text-left">Date</th>
            <th className="px-4 py-3 text-left">Books</th>
            <th className="px-4 py-3 text-left">Status</th>
            <th className="px-4 py-3 text-left">Details</th>
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={4} className="text-center py-4 text-gray-500">
                Loading requests...
              </td>
            </tr>
          ) : requests.length === 0 ? (
            <tr>
              <td colSpan={4} className="text-center py-4 text-gray-500">
                No requests yet.
              </td>
            </tr>
          ) : (
            sortedRequests.map((req) => (
              <tr key={req.id} className="odd:bg-white even:bg-gray-50">
                <td className="px-4 py-3">{formatDate(req.requestDate)}</td>
                <td className="px-4 py-3">{req.bookTitles.length} books</td>
                <td className="px-4 py-3">
                  <span
                    className={`px-2 py-1 rounded text-xs font-semibold ${
                      req.status === 0
                        ? "bg-yellow-500"
                        : req.status === 1
                        ? "bg-green-500"
                        : "bg-red-500"
                    } text-white`}
                  >
                    {statusOptions.find((s) => s.value === req.status)?.label}
                  </span>
                </td>
                <td className="px-4 py-3">
                  <button
                    onClick={() =>
                      toast.custom(
                        <BorrowDetailsToast
                          request={req}
                          getStatusText={getStatusText}
                        />,
                        { duration: 3000 }
                      )
                    }
                    className="text-indigo-500 hover:text-indigo-700 cursor-pointer"
                  >
                    View Details
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default BorrowRequestsTable;
