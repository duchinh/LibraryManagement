import React, { useEffect, useState } from "react";
import { getAllUsers, deleteUser } from "../../services/userService";
import { UserResponseDto } from "../../interfaces/user.interface";
import UpdateUserForm from "../../components/form/UpdateUserForm";
import UserManageTable from "../../components/table/UserManageTable";
import Modal from "../../components/Modal";
import toast from "react-hot-toast";

const UserManagePage: React.FC = () => {
  const [users, setUsers] = useState<UserResponseDto[]>([]);
  const [modalData, setModalData] = useState<{
    type: "edit" | "delete" | null;
    user: UserResponseDto | null;
  }>({ type: null, user: null });

  const loadUsers = async () => {
    try {
      const data = await getAllUsers();
      setUsers(data);
    } catch (error) {
      toast.error("An error occurred while fetching users");
    }
  };

  useEffect(() => {
    loadUsers();
  }, []);

  const handleDelete = async () => {
    if (modalData.user) {
      try {
        await deleteUser(modalData.user.id);
        setModalData({ type: null, user: null });
        loadUsers();
        toast.success("User deleted successfully");
      } catch (error) {
        toast.error("Failed to delete user");
      }
    }
  };

  const handleCloseModal = () => {
    setModalData({ type: null, user: null });
  };

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Users</h1>
      </div>

      <UserManageTable
        users={users}
        onEdit={(user) => setModalData({ type: "edit", user })}
        onDelete={(user) => setModalData({ type: "delete", user })}
      />

      <Modal
        isOpen={modalData.type !== null}
        onClose={handleCloseModal}
        title={modalData.type === "edit" ? "Edit User" : "Confirm Deletion"}
      >
        {modalData.type === "edit" && modalData.user && (
          <UpdateUserForm
            user={modalData.user}
            onUpdated={loadUsers}
            onCancel={handleCloseModal}
          />
        )}
        {modalData.type === "delete" && modalData.user && (
          <div>
            <p>
              Are you sure you want to delete{" "}
              <strong>
                {modalData.user.firstName} {modalData.user.lastName}
              </strong>
              ?
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

export default UserManagePage;
