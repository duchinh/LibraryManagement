export const ROOT_API = {
  baseURL: "http://localhost:5277",
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json",
  },
};

export const ENDPOINT_API = {
  auth: {
    login: "/api/Auth/login",
    register: "/api/Auth/register",
  },

  users: {
    getById: "/api/User/GetById/{id}",
    getAll: "/api/User/GetAll",
    update: "/api/User/Update",
    delete: "/api/User/Delete/{id}",
  },

  books: {
    getAll: "/api/Book",
    getById: "/api/Book/GetById/{id}",
    create: "/api/Book/Create",
    update: "/api/Book/Update/{id}",
    delete: "/api/Book/Delete/{id}",
  },

  categories: {
    getAll: "/api/Category/GetAll",
    getById: "/api/Category/GetById/{id}",
    create: "/api/Category/Create",
    update: "/api/Category/Update/{id}",
    delete: "/api/Category/Delete/{id}",
  },

  borrowings: {
    borrow: "/api/BorrowingRequests/Borrow",
    getAllRequests: "/api/BorrowingRequests/admin/requests",
    updateStatus: "/api/BorrowingRequests/admin/requests/{id}/status",
    getByUserId: "/api/BorrowingRequests/User/{id}/requests",
    getByRequestId: "/api/BorrowingRequestDetails/request/{requestId}",
  },

  admin: {
    dashboard: "/api/Dashboard/admin",
  },
};
