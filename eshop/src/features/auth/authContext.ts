import { createContext } from "react";
import { Claims } from "./authSlice";

export interface AuthContextProps {
    isAuthenticated: boolean,
    claims: Claims,
    signIn: () => Promise<void>,
    signOut: () => Promise<void>,
};

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export default AuthContext;