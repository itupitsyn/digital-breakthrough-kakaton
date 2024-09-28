import { TOKEN_COOKIES } from "@/constants/token";
import { Auth } from "@/model/auth";
import axios from "axios";
import { useCookies } from "next-client-cookies";
import { useRouter } from "next/navigation";
import { createContext, FC, PropsWithChildren, useCallback, useContext, useEffect, useState } from "react";
import toast from "react-hot-toast";

interface AuthContextType {
  token?: string;
  isAuth: boolean;
  isAuthenticating: boolean;
  auth: () => void;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider: FC<PropsWithChildren> = ({ children }) => {
  const [isAuth, setIsAuth] = useState(false);
  const [token, setToken] = useState<string>();
  const [isAuthenticating, setIsAuthentiacting] = useState(false);
  const { refresh } = useRouter();
  const cookies = useCookies();

  const auth = useCallback(async () => {
    try {
      setIsAuthentiacting(true);
      const response = await axios.post<Auth>("/api/auth");
      setIsAuth(true);
      setToken(response.data.token);
      cookies.set(TOKEN_COOKIES, response.data.token, {
        expires: 365,
      });
      toast.success("Успех!", { position: "bottom-center" });
      refresh();
    } catch {
      setIsAuth(false);
      setToken(undefined);
      cookies.remove(TOKEN_COOKIES);
      toast.error("Не удалось залогиниться", { position: "bottom-center" });
    } finally {
      setIsAuthentiacting(false);
    }
  }, [cookies, refresh]);

  useEffect(() => {
    const res = cookies.get(TOKEN_COOKIES);
    if (res) {
      setToken(res);
      setIsAuth(true);
    }
  }, [cookies]);

  return <AuthContext.Provider value={{ token, isAuth, auth, isAuthenticating }}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const cntx = useContext(AuthContext);

  return cntx;
};
