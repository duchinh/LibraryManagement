import { useRoutes } from "react-router-dom";
import { AppRouters } from "./routers";
import AuthProvider from "./contexts/AuthContext";
import { Toaster } from "react-hot-toast";
import UserProvider from "./contexts/UserContext";

function App() {
  let element = useRoutes(AppRouters);
  return (
    <div className="App">
      <AuthProvider>
        <UserProvider>
          <Toaster />
          {element}
        </UserProvider>
      </AuthProvider>
    </div>
  );
}

export default App;