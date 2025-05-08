import React, { useState, useEffect } from "react";
import { BookDTO } from "../../interfaces/book.interface";
import { updateBook } from "../../services/bookService";
import { getAllCategories } from "../../services/categoryService";
import toast from "react-hot-toast";

interface UpdateBookFormProps {
  book: BookDTO;
  onUpdated: () => void;
  onCancel: () => void;
}

const UpdateBookForm: React.FC<UpdateBookFormProps> = ({
  book,
  onUpdated,
  onCancel,
}) => {
  const [formData, setFormData] = useState({
    id: book.id,
    title: book.title,
    author: book.author,
    description: book.description || "",
    publicationYear: book.publicationYear || new Date().getFullYear(),
    categoryId: book.categoryId || "",
    totalCopies: book.totalCopies.toString(),
    availableCopies: book.availableCopies.toString(),
  });

  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [categories, setCategories] = useState<{ id: string; name: string }[]>(
    []
  );

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
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const validate = () => {
    const errs: { [key: string]: string } = {};
    if (!formData.title.trim()) errs.title = "Title is required";
    if (!formData.author.trim()) errs.author = "Author is required";
    if (!formData.categoryId) errs.categoryId = "Category is required";
    if (
      !formData.totalCopies ||
      isNaN(Number(formData.totalCopies)) ||
      Number(formData.totalCopies) <= 0
    ) {
      errs.totalCopies = "Total copies must be a positive number";
    }
    setErrors(errs);
    return Object.keys(errs).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    try {
      await updateBook(book.id, {
        id: book.id,
        title: formData.title,
        author: formData.author,
        description: formData.description,
        publicationYear: parseInt(formData.publicationYear.toString()),
        categoryId: formData.categoryId,
        totalCopies: parseInt(formData.totalCopies),
        availableCopies: parseInt(formData.availableCopies),
      });
      toast.success("Book updated successfully!");
      onUpdated();
    } catch (err) {
      toast.error("Failed to update book.");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-4 max-w-xl mx-auto p-4 bg-white shadow-lg rounded-md w-full h-full overflow-auto"
    >
      <h2 className="text-xl font-semibold mb-4 text-center">Update Book</h2>

      <div className="mb-4">
        <label className="block mb-1">Title</label>
        <input
          type="text"
          name="title"
          value={formData.title}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
        {errors.title && <p className="text-red-500 text-sm">{errors.title}</p>}
      </div>

      <div className="mb-4">
        <label className="block mb-1">Author</label>
        <input
          type="text"
          name="author"
          value={formData.author}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
        {errors.author && (
          <p className="text-red-500 text-sm">{errors.author}</p>
        )}
      </div>

      <div className="mb-4">
        <label className="block mb-1">Description</label>
        <textarea
          name="description"
          value={formData.description}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
          rows={4}
        ></textarea>
        {errors.description && (
          <p className="text-red-500 text-sm">{errors.description}</p>
        )}
      </div>

      <div className="mb-4">
        <label className="block mb-1">Publication Year</label>
        <input
          type="number"
          name="publicationYear"
          value={formData.publicationYear}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
        {errors.publicationYear && (
          <p className="text-red-500 text-sm">{errors.publicationYear}</p>
        )}
      </div>

      <div className="mb-4">
        <label className="block mb-1">Category</label>
        <select
          name="categoryId"
          value={formData.categoryId}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        >
          <option value="">Select Category</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        {errors.categoryId && (
          <p className="text-red-500 text-sm">{errors.categoryId}</p>
        )}
      </div>

      <div className="mb-6">
        <label className="block mb-1">Total Copies</label>
        <input
          type="number"
          name="totalCopies"
          value={formData.totalCopies}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
        {errors.totalCopies && (
          <p className="text-red-500 text-sm">{errors.totalCopies}</p>
        )}
      </div>

      <div className="mb-6">
        <label className="block mb-1">Available Copies</label>
        <input
          type="number"
          name="availableCopies"
          value={formData.availableCopies}
          onChange={handleChange}
          className="w-full border border-gray-300 rounded px-3 py-2"
        />
        {errors.availableCopies && (
          <p className="text-red-500 text-sm">{errors.availableCopies}</p>
        )}
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

export default UpdateBookForm;
