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
        let authority = window.location.origin;
        if (process.env.REACT_APP_ORIGIN) {
            authority = process.env.REACT_APP_ORIGIN;
        }

        const config: Oidc.UserManagerSettings = {
            authority: authority,
            client_id: "client",
            redirect_uri: `${authority}/auth/signIn/callback`,
            post_logout_redirect_uri: `${authority}/auth/signOut/callback`,
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
    }, [setClaims, setIsAuthenticated, setToken]);

    useEffect(() => {
        const listener = (user: Oidc.User) => {
            processUser(user);
        };

        manager.events.addUserLoaded(listener);
        return () => {
            manager.events.removeUserLoaded(listener);
        };
    }, [manager, processUser]);

    const getUser = useCallback(async () => {
        try {
            await manager.signinSilent();
        } catch (error: any) {
            processUser(null);
        }
    }, [manager, processUser]);

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
    }, [manager, processUser]);

    const processSignOutCallback = useCallback(async () => {
        const response = await manager.signoutRedirectCallback();

        const returnUrl = response.state?.returnUrl ?? "/";
        window.location.assign(returnUrl);
    }, [manager]);

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
    }, [manager]);

    const signOut = useCallback(async () => {
        await manager.signoutRedirect({
            state: {
                returnUrl: window.location.href,
            },
        });
    }, [manager]);

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