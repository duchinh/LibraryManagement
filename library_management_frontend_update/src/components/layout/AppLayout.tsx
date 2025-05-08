import Sidebar from "../Sidebar";
import { Outlet } from "react-router-dom";

const AppLayout = () => {
  return (
    <div className="flex">
      <Sidebar isAdmin={false} />
      <main className="ml-64 flex-1 h-screen overflow-y-auto p-6 bg-gray-100">
        <Outlet />
      </main>
    </div>
  );
};

export default AppLayout;
