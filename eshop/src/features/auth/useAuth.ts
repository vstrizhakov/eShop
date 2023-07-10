import React from "react";
import AuthContext, { AuthContextProps } from "./authContext";

const useAuth = (): AuthContextProps => {
    const context = React.useContext(AuthContext);

    if (!context) {
        throw new Error("AuthContext context is undefined, please verify you are calling useAuth() as child of a <AuthProvider> component.");
    }

    return context;
}

export default useAuth;