import React, { useEffect, useState } from "react";
import BorrowRequestsTable from "../../components/table/BorrowRequestsTable";
import { BorrowingRequestDTO } from "../../interfaces/borrow.interface";
import { getMyBorrowRequests } from "../../services/borrowingService";
import { useUserContext } from "../../contexts/UserContext";
import toast from "react-hot-toast";

const BorrowRequestsPage: React.FC = () => {
  const [requests, setRequests] = useState<BorrowingRequestDTO[]>([]);
  const [loadingRequests, setLoadingRequests] = useState(true);
  const [error, setError] = useState("");

  const { user, isLoading: userLoading } = useUserContext();
  const userId = user?.id || "";

  const getStatusText = (status: number) =>
    status === 0
      ? "Pending"
      : status === 1
      ? "Approved"
      : status === 2
      ? "Returned"
      : "Unknown";

  useEffect(() => {
    if (!userLoading && userId) {
      (async () => {
        try {
          const result = await getMyBorrowRequests(userId);
          setRequests(result);
        } catch (err) {
          console.error("Failed to load requests", err);
          setError("Failed to load your borrowing requests.");
          toast.error("Could not fetch borrow requests.");
        } finally {
          setLoadingRequests(false);
        }
      })();
    }
  }, [userLoading, userId]);

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-3xl font-bold mb-6">My Borrow Requests</h1>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <BorrowRequestsTable
        requests={requests}
        loading={loadingRequests}
        getStatusText={getStatusText}
      />
    </div>
  );
};

export default BorrowRequestsPage;
