import React from 'react';
import { Link } from 'react-router-dom';

const Sidebar: React.FC = () => {
  return (
    <aside className="bg-gray-800 text-white w-64 min-h-screen p-4">
      <nav>
        <ul className="space-y-2">
          <li><Link to="/user/dashboard" className="block p-2 hover:bg-gray-700 rounded">Dashboard</Link></li>
          <li><Link to="/user/books" className="block p-2 hover:bg-gray-700 rounded">Books</Link></li>
          <li><Link to="/user/borrow-requests" className="block p-2 hover:bg-gray-700 rounded">Borrow Requests</Link></li>
        </ul>
      </nav>
    </aside>
  );
};

export default Sidebar; 