import React from "react";
import toast from "react-hot-toast";
import { BorrowingRequestDTO } from "../../interfaces/borrow.interface";
import { Check, X } from "lucide-react";
import BorrowDetailsToast from "../BorrowDetailsToast";
import { formatDate } from "../../helpers/dateHelpers";

interface BorrowRequestManageTableProps {
  requests: BorrowingRequestDTO[];
  loading: boolean;
  handleStatusChange: (requestId: string, newStatus: number) => void;
}

const statusOptions = [
  { value: 0, label: "Pending" },
  { value: 1, label: "Approved" },
  { value: 2, label: "Rejected" },
];

const getStatusText = (status: number) => {
  return statusOptions.find((s) => s.value === status)?.label || "Unknown";
};

const BorrowRequestManageTable: React.FC<BorrowRequestManageTableProps> = ({
  requests,
  loading,
  handleStatusChange,
}) => {
  if (loading) {
    return <p className="text-center text-gray-500">Loading requests...</p>;
  }

  if (requests.length === 0) {
    return (
      <p className="text-center text-gray-500">No borrowing requests found.</p>
    );
  }

  const sortedRequests = [...requests].sort(
    (a, b) =>
      new Date(b.requestDate).getTime() - new Date(a.requestDate).getTime()
  );

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
        <thead className="bg-indigo-400">
          <tr>
            <th className="px-4 py-3 text-left text-white">Requestor</th>
            <th className="px-4 py-3 text-left text-white">Request Date</th>
            <th className="px-4 py-3 text-left text-white">Status</th>
            <th className="px-4 py-3 text-left text-white">Action</th>
            <th className="px-4 py-3 text-left text-white">Request Details</th>
          </tr>
        </thead>
        <tbody>
          {sortedRequests.map((req) => (
            <tr
              key={req.id}
              className="odd:bg-white even:bg-gray-50 hover:bg-gray-100"
            >
              <td className="px-4 py-3">
                {req.firstName} {req.lastName}
              </td>
              <td className="px-4 py-3">{formatDate(req.requestDate)}</td>
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
                  {getStatusText(req.status)}
                </span>
              </td>
              <td className="px-4 py-3">
                {req.status === 0 ? (
                  <div className="flex space-x-2">
                    <button
                      onClick={() => handleStatusChange(req.id, 1)}
                      className="p-2 bg-green-500 text-white rounded hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-500"
                      aria-label="Approve request"
                    >
                      <Check size={16} />
                    </button>
                    <button
                      onClick={() => handleStatusChange(req.id, 2)}
                      className="p-2 bg-red-500 text-white rounded hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-500"
                      aria-label="Reject request"
                    >
                      <X size={16} />
                    </button>
                  </div>
                ) : (
                  <span className="text-gray-400">No actions available</span>
                )}
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
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default BorrowRequestManageTable;
