import { createContext, useContext, useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";

interface User {
  id: string;
  userName: string;
  role: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: string;
  fullName?: string;
}

interface UserContextType {
  user: User | null;
  setUser: React.Dispatch<React.SetStateAction<User | null>>;
  isLoading: boolean;
}

const UserContext = createContext<UserContextType>({
  user: null,
  setUser: () => {},
  isLoading: true,
});

export const useUserContext = () => useContext(UserContext);

const UserProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token) {
      setIsLoading(true);
      try {
        const decoded: any = jwtDecode(token);
        const userDetails: User = {
          id: decoded.Id,
          userName: decoded.sub,
          role: decoded.Role,
          email: decoded.Email,
          firstName: decoded.FirstName,
          lastName: decoded.LastName,
          phoneNumber: decoded.PhoneNumber,
          address: decoded.Address,
          dateOfBirth: decoded.DateOfBirth,
          fullName: `${decoded.FirstName} ${decoded.LastName}`,
        };
        setUser(userDetails);
      } catch (error) {
        console.error("Error decoding token", error);
        setUser(null);
      }
      setIsLoading(false);
    } else {
      setIsLoading(false);
    }
  }, []);

  const contextValue = {
    user,
    setUser,
    isLoading,
  };

  return (
    <UserContext.Provider value={contextValue}>{children}</UserContext.Provider>
  );
};

export default UserProvider;
