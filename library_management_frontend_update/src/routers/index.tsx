import React from 'react';
import { Routes, Route } from 'react-router-dom';
import PrivateRoute from '../components/auth/PrivateRoute';
import Login from '../pages/auth/Login';
import Register from '../pages/auth/Register';
import UserDashboard from '../pages/user/Dashboard';
import AdminDashboard from '../pages/admin/Dashboard';
import BookList from '../pages/books/BookList';
import BookDetail from '../pages/books/BookDetail';
import BorrowRequests from '../pages/borrow/BorrowRequests';
import MyBorrowedBooks from '../pages/borrow/MyBorrowedBooks';
import Profile from '../pages/user/Profile';
import NotFound from '../pages/NotFound';

export default function AppRouter() {
  return (
    <Routes>
      {/* Public routes */}
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />

      {/* Protected routes */}
      <Route
        path="/"
        element={
          <PrivateRoute>
            <UserDashboard />
          </PrivateRoute>
        }
      />
      <Route
        path="/admin/dashboard"
        element={
          <PrivateRoute requiredRole="SuperUser">
            <AdminDashboard />
          </PrivateRoute>
        }
      />
      <Route
        path="/books"
        element={
          <PrivateRoute>
            <BookList />
          </PrivateRoute>
        }
      />
      <Route
        path="/books/:id"
        element={
          <PrivateRoute>
            <BookDetail />
          </PrivateRoute>
        }
      />
      <Route
        path="/admin/borrow-requests"
        element={
          <PrivateRoute requiredRole="SuperUser">
            <BorrowRequests />
          </PrivateRoute>
        }
      />
      <Route
        path="/borrow-requests"
        element={
          <PrivateRoute>
            <MyBorrowedBooks />
          </PrivateRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <PrivateRoute>
            <Profile />
          </PrivateRoute>
        }
      />

      {/* 404 route */}
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
} 