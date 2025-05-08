import React from "react";
import { CategoryDto } from "../../interfaces/category.interface";
import { Edit, Trash } from "lucide-react";

interface CategoryTableProps {
  categories: CategoryDto[];
  onEdit: (category: CategoryDto) => void;
  onDelete: (category: CategoryDto) => void;
}

const CategoryManageTable: React.FC<CategoryTableProps> = ({
  categories,
  onEdit,
  onDelete,
}) => {
  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
        <thead className="bg-indigo-400">
          <tr>
            {["Name", "Description", "Actions"].map((h) => (
              <th
                key={h}
                className="text-left px-4 py-3 text-sm font-bold text-white"
              >
                {h}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200">
          {categories.length > 0 ? (
            categories.map((category) => (
              <tr
                key={category.id}
                className="odd:bg-white even:bg-gray-50 hover:bg-gray-100 transition"
              >
                <td className="px-4 py-3 text-sm text-gray-900">
                  {category.name}
                </td>
                <td className="px-4 py-3 text-sm text-gray-700">
                  {category.description ?? "N/A"}
                </td>
                <td className="px-4 py-3 flex gap-3 items-center">
                  <button
                    onClick={() => onEdit(category)}
                    title="Edit"
                    className="text-indigo-500 cursor-pointer hover:text-indigo-700 w-5 h-5 transition"
                  >
                    <Edit />
                  </button>
                  <button
                    onClick={() => onDelete(category)}
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
                colSpan={3}
                className="text-center py-6 text-gray-500 text-sm"
              >
                No categories found.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default CategoryManageTable;
