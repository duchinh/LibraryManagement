import React, { useState, useEffect } from "react";
import { UserUpdateDto } from "../../interfaces/user.interface";
import toast from "react-hot-toast";
import { updateUser } from "../../services/userService";

interface UpdateUserFormProps {
  user: UserUpdateDto;
  onUpdated: () => void;
  onCancel: () => void;
}

const UpdateUserForm: React.FC<UpdateUserFormProps> = ({
  user,
  onUpdated,
  onCancel,
}) => {
  const [formData, setFormData] = useState<UserUpdateDto>(user);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setFormData(user);
  }, [user]);

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await toast.promise(updateUser(formData), {
        loading: "Updating user...",
        success: "User updated successfully",
        error: (err) =>
          err instanceof Error ? err.message : "An unknown error occurred",
      });
      onUpdated();
    } catch (error) {
      toast.error("An error occurred while updating user");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-4 grid grid-cols-1 sm:grid-cols-2 gap-4"
    >
      <div className="space-y-1">
        <label htmlFor="firstName" className="block text-sm text-gray-600">
          First Name
        </label>
        <input
          type="text"
          name="firstName"
          id="firstName"
          value={formData.firstName}
          onChange={handleInputChange}
          placeholder="Enter first name"
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="lastName" className="block text-sm text-gray-600">
          Last Name
        </label>
        <input
          type="text"
          name="lastName"
          id="lastName"
          value={formData.lastName}
          onChange={handleInputChange}
          placeholder="Enter last name"
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="email" className="block text-sm text-gray-600">
          Email
        </label>
        <input
          type="email"
          name="email"
          id="email"
          value={formData.email}
          onChange={handleInputChange}
          placeholder="Enter email address"
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="userName" className="block text-sm text-gray-600">
          Username
        </label>
        <input
          type="text"
          name="userName"
          id="userName"
          value={formData.userName}
          onChange={handleInputChange}
          placeholder="Enter username"
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="phoneNumber" className="block text-sm text-gray-600">
          Phone Number
        </label>
        <input
          type="text"
          name="phoneNumber"
          id="phoneNumber"
          value={formData.phoneNumber}
          onChange={handleInputChange}
          placeholder="Enter phone number"
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="address" className="block text-sm text-gray-600">
          Address
        </label>
        <input
          type="text"
          name="address"
          id="address"
          value={formData.address}
          onChange={handleInputChange}
          placeholder="Enter address"
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="dateOfBirth" className="block text-sm text-gray-600">
          Date of Birth
        </label>
        <input
          type="datetime-local"
          name="dateOfBirth"
          id="dateOfBirth"
          value={formData.dateOfBirth.slice(0, 16)}
          onChange={handleInputChange}
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        />
      </div>

      <div className="space-y-1">
        <label htmlFor="role" className="block text-sm text-gray-600">
          Role
        </label>
        <select
          name="role"
          id="role"
          value={formData.role}
          onChange={(e) =>
            setFormData({ ...formData, role: parseInt(e.target.value) })
          }
          className="w-full p-2 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500"
        >
          <option value="0">User</option>
          <option value="1">Admin</option>
        </select>
      </div>

      <div className="space-y-1 col-span-2">
        <div className="flex items-center space-x-2">
          <input
            type="checkbox"
            name="isActive"
            id="isActive"
            checked={formData.isActive}
            onChange={(e) =>
              setFormData({ ...formData, isActive: e.target.checked })
            }
            className="h-4 w-4 text-blue-500 focus:ring-1 focus:ring-blue-500"
          />
          <label htmlFor="isActive" className="text-sm text-gray-600">
            Active
          </label>
        </div>
      </div>

      <div className="flex justify-end gap-4 mt-4 col-span-2">
        <button
          type="button"
          onClick={onCancel}
          className="bg-gray-200 hover:bg-gray-300 text-sm text-gray-700 px-4 py-2 rounded-md transition duration-200"
        >
          Cancel
        </button>
        <button
          type="submit"
          disabled={loading}
          className={`${
            loading
              ? "bg-blue-300 cursor-not-allowed"
              : "bg-blue-500 hover:bg-blue-600"
          } text-white text-sm px-6 py-2 rounded-md transition duration-200`}
        >
          {loading ? "Updating..." : "Update User"}
        </button>
      </div>
    </form>
  );
};

export default UpdateUserForm;
