import { createContext, useState, ReactNode } from "react";

interface AuthContextType {
  user: { email: string } | null;
  token: string | null;
  login: (email: string, token: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const getSavedUser = () => {
  const savedUser = localStorage.getItem("user");
  if (savedUser) {
    return JSON.parse(savedUser);
  }
  return null;
};

const getSavedToken = () => {
  return localStorage.getItem("token");
};

const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<{ email: string } | null>(getSavedUser());
  const [token, setToken] = useState<string | null>(getSavedToken());


  const login = (email: string, token: string) => {
    setUser({ email });
    setToken(token);
    localStorage.setItem("user", JSON.stringify({ email }));
    localStorage.setItem("token", token);
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem("user");
    localStorage.removeItem("token");
  };

  return (
    <AuthContext.Provider value={{ user, token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthContext, AuthProvider };
