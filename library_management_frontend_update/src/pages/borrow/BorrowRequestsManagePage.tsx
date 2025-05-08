import React, { useEffect, useState } from "react";
import { BorrowingRequestDTO } from "../../interfaces/borrow.interface";
import { toast } from "react-hot-toast";
import {
  getAllBorrowRequests,
  updateBorrowRequestStatus,
} from "../../services/borrowingService";
import BorrowRequestManageTable from "../../components/table/BorrowRequestManageTable";

const BorrowRequestsManagePage: React.FC = () => {
  const [requests, setRequests] = useState<BorrowingRequestDTO[]>([]);
  const [loading, setLoading] = useState(false);

  const fetchRequests = async () => {
    try {
      setLoading(true);
      const data = await getAllBorrowRequests();
      setRequests(data);
    } catch (error) {
      toast.error("Failed to load borrowing requests.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  const handleStatusChange = async (requestId: string, newStatus: number) => {
    try {
      await updateBorrowRequestStatus(requestId, newStatus);
      toast.success("Request status updated successfully!");
      await fetchRequests();
    } catch (error) {
      toast.error("Failed to update status.");
    }
  };

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <h1 className="text-3xl font-bold mb-6 text-gray-800">
        Manage Borrowing Requests
      </h1>
      <BorrowRequestManageTable
        requests={requests}
        loading={loading}
        handleStatusChange={handleStatusChange}
      />
    </div>
  );
};

export default BorrowRequestsManagePage;
