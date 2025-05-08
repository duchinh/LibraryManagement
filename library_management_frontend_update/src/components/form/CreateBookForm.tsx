import React, { useState, useEffect } from "react";
import { CreateBookDTO } from "../../interfaces/book.interface";
import { createBook } from "../../services/bookService";
import { getAllCategories } from "../../services/categoryService";
import { toast } from "react-hot-toast";

interface Props {
  onCreated: () => void;
  onCancel: () => void;
}

const initialForm: CreateBookDTO = {
  title: "",
  author: "",
  description: "",
  publicationYear: new Date().getFullYear(),
  totalCopies: 1,
  categoryId: "",
};

const CreateBookForm: React.FC<Props> = ({ onCreated, onCancel }) => {
  const [form, setForm] = useState<CreateBookDTO>(initialForm);
  const [categories, setCategories] = useState<{ id: string; name: string }[]>(
    []
  );
  const [error, setError] = useState<string>("");

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await getAllCategories();
        setCategories(data);
      } catch (err) {
        console.error("Failed to fetch categories:", err);
      }
    };

    fetchCategories();
  }, []);

  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement
    >
  ) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]:
        name === "publicationYear" || name === "totalCopies"
          ? parseInt(value)
          : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!form.title || !form.author || !form.categoryId) {
      setError("All fields are required.");
      return;
    }

    if (form.publicationYear <= 0 || form.totalCopies <= 0) {
      setError("Publication year and total copies must be positive numbers.");
      return;
    }

    try {
      await createBook(form);

      toast.success("Book created successfully!");

      setForm(initialForm);
      setError("");

      onCreated();
    } catch (err) {
      console.error("Failed to create book:", err);

      toast.error("An error occurred while creating the book.");
      setError("An error occurred while creating the book.");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-6 border p-8 rounded-lg shadow-lg w-full max-w-md bg-white"
    >
      <h2 className="text-2xl font-semibold text-center text-gray-800">
        Create New Book
      </h2>

      {error && (
        <div className="text-red-600 text-sm text-center mb-4">{error}</div>
      )}

      <div className="space-y-4">
        <div>
          <label
            htmlFor="title"
            className="block text-sm font-medium text-gray-700"
          >
            Title
          </label>
          <input
            id="title"
            name="title"
            value={form.title}
            onChange={handleChange}
            placeholder="Enter book title"
            required
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label
            htmlFor="author"
            className="block text-sm font-medium text-gray-700"
          >
            Author
          </label>
          <input
            id="author"
            name="author"
            value={form.author}
            onChange={handleChange}
            placeholder="Enter author's name"
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
            placeholder="Enter book description"
            rows={4}
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label
            htmlFor="publicationYear"
            className="block text-sm font-medium text-gray-700"
          >
            Publication Year
          </label>
          <input
            id="publicationYear"
            name="publicationYear"
            type="number"
            value={form.publicationYear}
            onChange={handleChange}
            placeholder="Enter publication year"
            required
            min={1000}
            max={new Date().getFullYear()}
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label
            htmlFor="totalCopies"
            className="block text-sm font-medium text-gray-700"
          >
            Total Copies
          </label>
          <input
            id="totalCopies"
            name="totalCopies"
            type="number"
            value={form.totalCopies}
            onChange={handleChange}
            placeholder="Enter total copies"
            required
            min={1}
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label
            htmlFor="categoryId"
            className="block text-sm font-medium text-gray-700"
          >
            Category
          </label>
          <select
            id="categoryId"
            name="categoryId"
            value={form.categoryId}
            onChange={handleChange}
            required
            className="w-full border-gray-300 rounded-md shadow-sm p-3 focus:ring-2 focus:ring-blue-500"
          >
            <option value="">Select Category</option>
            {categories.map((category) => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
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

export default CreateBookForm;
