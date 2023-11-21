import React, { PropsWithChildren, useCallback, useEffect, useMemo } from "react";
import AuthContext from "./authContext";
import * as Oidc from "oidc-client";
import { connect, ConnectedProps } from "react-redux";
import { RootState } from "../../app/store";
import { setClaims, setIsAuthenticated, setToken } from "./authSlice";
import { Spinner } from "react-bootstrap";

const mapStateToProps = (state: RootState) => ({
    isAuthenticated: state.auth.isAuthenticated,
    claims: state.auth.claims,
});

const mapDispatchToProps = {
    setIsAuthenticated,
    setToken,
    setClaims,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type ReduxProps = ConnectedProps<typeof connector>;

const AuthProvider: React.FC<PropsWithChildren<ReduxProps>> = (props) => {
    const {
        isAuthenticated,
        claims,
        children,
        setIsAuthenticated,
        setToken,
        setClaims,
    } = props;

    const manager = useMemo(() => {
        const config: Oidc.UserManagerSettings = {
            authority: window.location.origin,
            client_id: "client",
            redirect_uri: `${window.location.origin}/auth/signIn/callback`,
            post_logout_redirect_uri: `${window.location.origin}/auth/signOut/callback`,
            response_type: "code",
            scope: "openid profile phone account api",
            automaticSilentRenew: true,
        };

        Oidc.Log.level = Oidc.Log.DEBUG;
        Oidc.Log.logger = console;

        return new Oidc.UserManager(config);
    }, []);

    const pathname = window.location.pathname;

    const processUser = useCallback((user: Oidc.User | null) => {
        const isAuthenticated = user !== null;
        if (isAuthenticated) {
            setToken(user.access_token);
            setClaims(user.profile);
        }
        setIsAuthenticated(isAuthenticated);
    }, []);

    const getUser = useCallback(async () => {
        let user: Oidc.User | null = null;

        try {
            user = await manager.signinSilent();
        } catch (error: any) {
        }

        processUser(user);
    }, [processUser]);

    const processSignInCallback = useCallback(async () => {
        try {
            const user = await manager.signinCallback();

            processUser(user);

            const state = (user?.state as any);
            if (state?.returnUrl) {
                window.location.assign(state.returnUrl);
            }
        } catch (error) {
        }
    }, [pathname, processUser]);

    const processSignOutCallback = useCallback(async () => {
        const response = await manager.signoutRedirectCallback();

        const returnUrl = response.state?.returnUrl ?? "/";
        window.location.assign(returnUrl);
    }, []);

    useEffect(() => {
        if (pathname === "/auth/signIn/callback") {
            processSignInCallback();
        } else if (pathname === "/auth/signOut/callback") {
            processSignOutCallback();
        } else {
            getUser();
        }
    }, [pathname, getUser, processSignInCallback, processSignOutCallback]);

    const signIn = useCallback(async () => {
        await manager.signinRedirect({
            state: {
                returnUrl: window.location.href,
            },
        });
    }, []);

    const signOut = useCallback(async () => {
        await manager.signoutRedirect({
            state: {
                returnUrl: window.location.href,
            },
        });
    }, []);

    return (
        <>
            {isAuthenticated === undefined && (
                <Spinner />
            )}
            {isAuthenticated !== undefined && (
                <AuthContext.Provider value={{
                    isAuthenticated: isAuthenticated,
                    claims: claims!,
                    signIn: signIn,
                    signOut: signOut,
                }}>
                    {children}
                </AuthContext.Provider>
            )}
        </>
    );
};

export default connector(AuthProvider);