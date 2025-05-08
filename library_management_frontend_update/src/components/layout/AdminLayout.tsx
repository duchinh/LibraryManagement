import Sidebar from "../Sidebar";
import { Outlet } from "react-router-dom";

const AdminLayout = () => {
  return (
    <div className="flex">
      <Sidebar isAdmin={true} />
      <main className="ml-64 flex-1 h-screen overflow-y-auto p-6 bg-gray-100">
        <Outlet />
      </main>
    </div>
  );
};

export default AdminLayout;
