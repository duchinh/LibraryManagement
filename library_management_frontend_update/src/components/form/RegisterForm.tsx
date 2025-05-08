import { useState } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import { register } from "../../services/authService";

const InputField = ({
  label,
  name,
  value,
  onChange,
  type = "text",
  ...rest
}: {
  label: string;
  name: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  type?: string;
  [key: string]: any;
}) => (
  <div>
    <label htmlFor={name} className="block text-sm font-medium text-gray-700">
      {label}
    </label>
    <input
      id={name}
      name={name}
      type={type}
      required
      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm text-gray-700"
      value={value}
      onChange={onChange}
      {...rest}
    />
  </div>
);

const RegisterForm = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    password: "",
    confirmPassword: "",
    phoneNumber: "",
    address: "",
    dateOfBirth: "",
  });
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (formData.password !== formData.confirmPassword) {
      toast.error("Passwords do not match");
      return;
    }

    setLoading(true);
    try {
      const response = await register(formData);
      if (response.success) {
        toast.success("Registration successful");
        navigate("/login");
      }
    } catch (error: any) {
      const validationErrors = error.response?.data?.errors;
      if (validationErrors) {
        const messages = Object.values(validationErrors).flat() as string[];
        messages.forEach((msg) => toast.error(msg));
      } else {
        const fallbackMessage =
          error.response?.data?.message ||
          error.response?.data?.title ||
          "Registration failed";
        toast.error(fallbackMessage);
      }
      console.error("Registration error:", error.response?.data || error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
      <div className="space-y-4">
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <InputField
            label="First Name"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
          />
          <InputField
            label="Last Name"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
          />
        </div>
        <InputField
          label="Username"
          name="userName"
          value={formData.userName}
          onChange={handleChange}
          pattern="^[a-zA-Z0-9_]+$"
          title="Username can only contain letters, numbers, and underscores"
          minLength={3}
          maxLength={30}
        />
        <InputField
          label="Email"
          name="email"
          type="email"
          value={formData.email}
          onChange={handleChange}
        />
        <InputField
          label="Password"
          name="password"
          type="password"
          value={formData.password}
          onChange={handleChange}
          pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$"
          title="Password must contain at least one uppercase letter, one lowercase letter, one number and one special character"
          minLength={6}
        />
        <InputField
          label="Confirm Password"
          name="confirmPassword"
          type="password"
          value={formData.confirmPassword}
          onChange={handleChange}
        />
        <InputField
          label="Phone Number"
          name="phoneNumber"
          type="tel"
          value={formData.phoneNumber}
          onChange={handleChange}
        />
        <InputField
          label="Address"
          name="address"
          value={formData.address}
          onChange={handleChange}
        />
        <InputField
          label="Date of Birth"
          name="dateOfBirth"
          type="date"
          value={formData.dateOfBirth}
          onChange={handleChange}
        />
      </div>

      <div>
        <button
          type="submit"
          disabled={loading}
          className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:bg-indigo-400"
        >
          {loading ? (
            <span className="flex items-center">
              <svg
                className="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
              >
                <circle
                  className="opacity-25"
                  cx="12"
                  cy="12"
                  r="10"
                  stroke="currentColor"
                  strokeWidth="4"
                />
                <path
                  className="opacity-75"
                  fill="currentColor"
                  d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"
                />
              </svg>
              Creating account...
            </span>
          ) : (
            "Create account"
          )}
        </button>
      </div>

      <div className="text-sm text-gray-500">
        <p>Password requirements:</p>
        <ul className="list-disc list-inside pl-2">
          <li>At least 6 characters long</li>
          <li>Must contain at least one uppercase letter</li>
          <li>Must contain at least one lowercase letter</li>
          <li>Must contain at least one number</li>
          <li>Must contain at least one special character (@$!%*?&)</li>
        </ul>
      </div>
    </form>
  );
};

export default RegisterForm;
