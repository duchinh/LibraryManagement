import { Link } from "react-router-dom";
import AuthLayout from "../../components/layout/AuthLayout";
import RegisterForm from "../../components/form/RegisterForm";

const RegisterPage = () => {
  return (
    <AuthLayout backgroundImage="https://images.unsplash.com/photo-1481627834876-b7833e8f5570?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80">
      <div className="space-y-6 py-2 px-6">
        <h2 className="text-4xl font-bold text-gray-900 text-center">
          Create your account
        </h2>
        <p className="text-center text-sm text-gray-600">
          Already have an account?{" "}
          <Link
            to="/login"
            className="font-medium text-indigo-600 hover:text-indigo-500"
          >
            Sign in
          </Link>
        </p>
        <RegisterForm />
      </div>
    </AuthLayout>
  );
};

export default RegisterPage;
