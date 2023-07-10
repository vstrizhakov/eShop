import { AuthContextProps } from "./authContext";
import useAuth from "./useAuth";
import React from "react";

export const withAuth = <P extends AuthContextProps>(Component: React.ComponentType<P>): React.ComponentType<Omit<P, keyof AuthContextProps>> => {
    const displayName = `withAuth(${Component.displayName})`;
    const WrappedComponent: React.FC<Omit<P, keyof AuthContextProps>> = (props) => {
        const auth = useAuth();

        return <Component {...(props as P)} {...auth} />;
    };

    WrappedComponent.displayName = displayName;

    return WrappedComponent;
};