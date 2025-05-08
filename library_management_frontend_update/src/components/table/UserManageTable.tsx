import React from "react";
import { UserResponseDto } from "../../interfaces/user.interface";
import { Edit, Trash } from "lucide-react"; // Lucide icons

interface UserManageTableProps {
  users: UserResponseDto[];
  onEdit: (user: UserResponseDto) => void;
  onDelete: (user: UserResponseDto) => void;
}

const UserManageTable: React.FC<UserManageTableProps> = ({
  users,
  onEdit,
  onDelete,
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
        <thead className="bg-indigo-400">
          <tr>
            {["Name", "Email", "Role", "Actions"].map((header) => (
              <th
                key={header}
                className="text-left px-4 py-3 text-sm font-bold text-white"
              >
                {header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200">
          {users.length > 0 ? (
            users.map((user) => (
              <tr
                key={user.id}
                className="odd:bg-white even:bg-gray-50 hover:bg-gray-100 transition"
              >
                <td className="px-4 py-3 text-sm text-gray-900">
                  {`${user.firstName} ${user.lastName}`}
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  {user.email}
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  {user.role === 1 ? "Admin" : "User"}
                </td>
                <td className="px-4 py-3 flex gap-3 items-center">
                  <button
                    onClick={() => onEdit(user)}
                    title="Edit"
                    className="text-indigo-500 cursor-pointer hover:text-indigo-700 w-5 h-5 transition"
                  >
                    <Edit />
                  </button>
                  <button
                    onClick={() => onDelete(user)}
                    title="Delete"
                    className="text-red-500 cursor-pointer hover:text-red-700 w-5 h-5 transition"
                  >
                    <Trash />
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td
                colSpan={4}
                className="text-center py-6 text-gray-500 text-sm"
              >
                No users found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default UserManageTable;
