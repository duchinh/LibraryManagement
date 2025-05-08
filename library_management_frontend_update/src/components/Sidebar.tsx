import {
  Book,
  LayoutDashboard,
  ClipboardList,
  Tag,
  LogOut,
} from "lucide-react";
import { useAuthContext } from "../contexts/AuthContext";
import { useUserContext } from "../contexts/UserContext";

const Sidebar = ({ isAdmin }: { isAdmin: boolean }) => {
  const { isAuthenticated, logout } = useAuthContext();
  const { user, isLoading: userLoading } = useUserContext();
  const isLoading = userLoading;

  const handleLogout = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
    await logout();
  };

  return (
    <aside className="fixed top-0 left-0 w-64 bg-white shadow-md h-screen flex flex-col">
      <div className="h-16 flex items-center justify-center bg-indigo-400 text-white text-xl font-bold">
        Library System
      </div>

      <nav className="flex-1 mt-4">
        <ul>
          {!isAdmin && (
            <>
              <li>
                <a
                  href="/"
                  className="flex items-center py-3 px-4 text-gray-600 hover:bg-gray-100"
                >
                  <LayoutDashboard className="w-5 h-5" />
                  <span className="ml-2">Home</span>
                </a>
              </li>
              <li>
                <a
                  href="/borrow-request"
                  className="flex items-center py-3 px-4 text-gray-600 hover:bg-gray-100"
                >
                  <ClipboardList className="w-5 h-5" />
                  <span className="ml-2">My Borrowings</span>
                </a>
              </li>
            </>
          )}

          {isAdmin && (
            <>
              <li>
                <a
                  href="/admin/dashboard"
                  className="flex items-center py-3 px-4 text-gray-600 hover:bg-gray-100"
                >
                  <LayoutDashboard className="w-5 h-5" />
                  <span className="ml-2">Dashboard</span>
                </a>
              </li>
              <li>
                <a
                  href="/admin/users"
                  className="flex items-center py-3 px-4 text-gray-600 hover:bg-gray-100"
                >
                  <ClipboardList className="w-5 h-5" />
                  <span className="ml-2">User Management</span>
                </a>
              </li>
              <li>
                <a
                  href="/admin/categories"
                  className="flex items-center py-3 px-4 text-gray-600 hover:bg-gray-100"
                >
                  <Tag className="w-5 h-5" />
                  <span className="ml-2">Categories Management</span>
                </a>
              </li>
              <li>
                <a
                  href="/admin/books"
                  className="flex items-center py-3 px-4 hover:bg-gray-100 text-gray-600"
                >
                  <Book className="w-5 h-5" />
                  <span className="ml-2">Book Management</span>
                </a>
              </li>
              <li>
                <a
                  href="/admin/requests"
                  className="flex items-center py-3 px-4 text-gray-600 hover:bg-gray-100"
                >
                  <ClipboardList className="w-5 h-5" />
                  <span className="ml-2">Borrow Requests</span>
                </a>
              </li>
            </>
          )}
        </ul>
      </nav>

      <footer className="py-4 px-4 bg-gray-50">
        <div className="flex items-center justify-between text-sm">
          <div>
            {isLoading ? (
              <p className="text-gray-500">Loading...</p>
            ) : (
              <>
                {isAuthenticated && user ? (
                  <>
                    <p className="font-medium text-gray-800">{user.fullName}</p>
                  </>
                ) : (
                  <p className="font-medium text-gray-800">Guest</p>
                )}
              </>
            )}
          </div>
          <button
            onClick={handleLogout}
            className="text-gray-600 hover:text-gray-900 cursor-pointer"
          >
            <LogOut className="w-5 h-5" />
          </button>
        </div>
      </footer>
    </aside>
  );
};

export default Sidebar;
