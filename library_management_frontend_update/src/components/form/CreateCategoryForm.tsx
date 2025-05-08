import React, { useState } from "react";
import { CreateCategoryDto } from "../../interfaces/category.interface";
import { createCategory } from "../../services/categoryService";
import { toast } from "react-hot-toast";

interface Props {
  onCreated: () => void;
  onCancel: () => void;
}

const initialForm: CreateCategoryDto = {
  name: "",
  description: "",
};

const CreateCategoryForm: React.FC<Props> = ({ onCreated, onCancel }) => {
  const [form, setForm] = useState<CreateCategoryDto>(initialForm);
  const [error, setError] = useState<string>("");

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!form.name.trim()) {
      setError("Name is required.");
      return;
    }

    try {
      await createCategory(form);
      toast.success("Category created successfully!");

      setForm(initialForm);
      setError("");
      onCreated();
    } catch (err) {
      console.error("Failed to create category:", err);
      toast.error("An error occurred while creating the category.");
      setError("An error occurred while creating the category.");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-6 border p-8 rounded-lg shadow-lg w-full max-w-md bg-white"
    >
      <h2 className="text-2xl font-semibold text-center text-gray-800">
        Create New Category
      </h2>

      {error && (
        <div className="text-red-600 text-sm text-center mb-4">{error}</div>
      )}

      <div className="space-y-4">
        <div>
          <label
            htmlFor="name"
            className="block text-sm font-medium text-gray-700"
          >
            Name
          </label>
          <input
            id="name"
            name="name"
            value={form.name}
            onChange={handleChange}
            placeholder="Enter category name"
            required
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label
            htmlFor="description"
            className="block text-sm font-medium text-gray-700"
          >
            Description
          </label>
          <textarea
            id="description"
            name="description"
            value={form.description}
            onChange={handleChange}
            placeholder="Enter category description"
            rows={4}
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          />
        </div>
      </div>

      <div className="flex gap-4">
        <button
          type="submit"
          className="bg-blue-600 text-white px-6 py-3 rounded-md w-full hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 transition"
        >
          Create
        </button>
        <button
          type="button"
          onClick={(e) => {
            e.preventDefault();
            onCancel();
          }}
          className="bg-gray-300 text-gray-700 px-6 py-3 rounded-md w-full hover:bg-gray-400 focus:ring-2 focus:ring-gray-500 transition"
        >
          Cancel
        </button>
      </div>
    </form>
  );
};

export default CreateCategoryForm;
