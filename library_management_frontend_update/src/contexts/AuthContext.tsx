import { jwtDecode } from "jwt-decode";
import {
  createContext,
  ReactNode,
  useContext,
  useEffect,
  useState,
} from "react";

interface JwtPayload {
  Role?: string;
  exp?: number;
}

interface AuthContextType {
  isAuthenticated: boolean;
  setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
  userRole: string | null;
  setUserRole: React.Dispatch<React.SetStateAction<string | null>>;
  isLoading: boolean;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  setIsAuthenticated: () => {},
  userRole: null,
  setUserRole: () => {},
  isLoading: true,
  logout: async () => {},
});

export const useAuthContext = () => useContext(AuthContext);

const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const token = localStorage.getItem("token");
  const [isAuthenticated, setIsAuthenticated] = useState(!!token);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (token) {
      try {
        const decoded = jwtDecode<JwtPayload>(token);

        if (decoded.exp && Date.now() >= decoded.exp * 1000) {
          localStorage.removeItem("token");
          setIsAuthenticated(false);
          setUserRole(null);
        } else {
          const role = decoded.Role;
          setIsAuthenticated(true);
          setUserRole(role || null);
        }
      } catch (err) {
        console.error("Invalid token");
        localStorage.removeItem("token");
        setIsAuthenticated(false);
        setUserRole(null);
      }
    }
    setIsLoading(false);
  }, [token]);

  const logout = async () => {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    setIsAuthenticated(false);
    setUserRole(null);
  };

  const contextValue = {
    isAuthenticated,
    setIsAuthenticated,
    userRole,
    setUserRole,
    isLoading,
    logout,
  };

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export default AuthProvider;
