import React, { useEffect, useState } from "react";
import { getAllCategories, deleteCategory } from "../../services/categoryService";
import { CategoryDto } from "../../interfaces/category.interface";
import CreateCategoryForm from "../../components/form/CreateCategoryForm";
import UpdateCategoryForm from "../../components/form/UpdateCategoryForm";
import { Plus } from "lucide-react";
import Pagination from "../../components/Pagination";
import CategoryManageTable from "../../components/table/CategoryManageTable";
import Modal from "../../components/Modal";
import toast from "react-hot-toast";

const CategoryManagePage: React.FC = () => {
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [modalData, setModalData] = useState<{
    type: "create" | "edit" | "delete" | null;
    category: CategoryDto | null;
  }>({ type: null, category: null });

  const [pagination, setPagination] = useState({
    currentPage: 1,
    itemsPerPage: 10,
  });

  const loadCategories = async () => {
    const data = await getAllCategories();
    setCategories(data);
  };

  useEffect(() => {
    loadCategories();
  }, []);

  const handlePaginationChange = (
    field: "currentPage" | "itemsPerPage",
    value: number
  ) => {
    if (field === "itemsPerPage") {
      setPagination((prev) => ({
        ...prev,
        itemsPerPage: value,
        currentPage: 1,
      }));
    } else {
      setPagination((prev) => ({
        ...prev,
        currentPage: value,
      }));
    }
  };

  const handleDelete = async () => {
    if (modalData.category) {
      await deleteCategory(modalData.category.id);
      setModalData({ type: null, category: null });
      toast.success("Category deleted successfully!");
      loadCategories();
    }
  };

  const handleCloseModal = () => {
    setModalData({ type: null, category: null });
  };

  const paginatedCategories = categories.slice(
    (pagination.currentPage - 1) * pagination.itemsPerPage,
    pagination.currentPage * pagination.itemsPerPage
  );

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Categories</h1>
        <button
          onClick={() => setModalData({ type: "create", category: null })}
          className="flex items-center gap-2 bg-indigo-400 text-white px-4 py-2 rounded hover:bg-indigo-600 cursor-pointer transition"
        >
          <Plus className="w-5 h-5" />
          Create Category
        </button>
      </div>

      <CategoryManageTable
        categories={paginatedCategories}
        onEdit={(category) => setModalData({ type: "edit", category })}
        onDelete={(category) => setModalData({ type: "delete", category })}
      />

      <Pagination
        currentPage={pagination.currentPage}
        itemsPerPage={pagination.itemsPerPage}
        totalRecords={categories.length}
        totalPages={Math.ceil(categories.length / pagination.itemsPerPage)}
        onPageChange={(page) => handlePaginationChange("currentPage", page)}
        onItemsPerPageChange={(event) =>
          handlePaginationChange("itemsPerPage", parseInt(event.target.value))
        }
        onPrevious={() =>
          handlePaginationChange("currentPage", pagination.currentPage - 1)
        }
        onNext={() =>
          handlePaginationChange("currentPage", pagination.currentPage + 1)
        }
      />

      <Modal
        isOpen={modalData.type !== null}
        onClose={handleCloseModal}
        title={
          modalData.type === "create"
            ? "Create Category"
            : modalData.type === "edit"
            ? "Edit Category"
            : "Confirm Deletion"
        }
      >
        {modalData.type === "create" && (
          <CreateCategoryForm
            onCreated={loadCategories}
            onCancel={handleCloseModal}
          />
        )}
        {modalData.type === "edit" && modalData.category && (
          <UpdateCategoryForm
            category={modalData.category}
            onUpdated={loadCategories}
            onCancel={handleCloseModal}
          />
        )}
        {modalData.type === "delete" && modalData.category && (
          <div>
            <p>
              Are you sure you want to delete{" "}
              <strong>{modalData.category.name}</strong>?
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

export default CategoryManagePage;
