import React, { useState } from "react";
import { UpdateCategoryDto } from "../../interfaces/category.interface";
import { updateCategory } from "../../services/categoryService";
import toast from "react-hot-toast";

interface UpdateCategoryFormProps {
  category: UpdateCategoryDto;
  onUpdated: () => void;
  onCancel: () => void;
}

const UpdateCategoryForm: React.FC<UpdateCategoryFormProps> = ({
  category,
  onUpdated,
  onCancel,
}) => {
  const [formData, setFormData] = useState<UpdateCategoryDto>({
    id: category.id,
    name: category.name,
    description: category.description || "",
  });

  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const validate = () => {
    const errs: { [key: string]: string } = {};
    if (!formData.name.trim()) errs.name = "Name is required";
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    try {
      await updateCategory(formData.id, formData);
      toast.success("Category updated successfully!");
      onUpdated();
    } catch (err) {
      console.error("Failed to update category:", err);
      toast.error("Failed to update category.");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-4 max-w-md mx-auto p-6 bg-white shadow-md rounded-md"
    >
      <h2 className="text-xl font-semibold mb-4 text-center">
        Update Category
      </h2>

      <div className="mb-4">
        <label className="block mb-1">Name</label>
        <input
          type="text"
          name="name"
          value={formData.name}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
        {errors.name && <p className="text-red-500 text-sm">{errors.name}</p>}
      </div>

      <div className="mb-4">
        <label className="block mb-1">Description</label>
        <textarea
          name="description"
          value={formData.description}
          onChange={handleChange}
          rows={4}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
      </div>

      <div className="flex justify-between gap-4 mt-6">
        <button
          type="button"
          onClick={onCancel}
          className="bg-gray-200 px-4 py-2 rounded hover:bg-gray-300 w-full sm:w-auto"
        >
          Cancel
        </button>
        <button
          type="submit"
          className="bg-indigo-600 text-white px-4 py-2 rounded hover:bg-indigo-700 w-full sm:w-auto"
        >
          Update
        </button>
      </div>
    </form>
  );
};

export default UpdateCategoryForm;
