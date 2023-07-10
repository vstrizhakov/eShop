import React, { PropsWithChildren, useEffect, useState } from "react";
import { Spinner } from "react-bootstrap";
import { AuthContextProps } from "./authContext";
import { Claims } from "./authSlice";
import { withAuth } from "./withAuth";

interface IProps extends AuthContextProps {
    handle: (claims: Claims) => boolean,
};

const Authorize: React.FC<PropsWithChildren<IProps>> = props => {
    const {
        claims,
        handle,
        children,
    } = props;

    const [isAuthorized, setIsAuthorized] = useState<boolean | undefined>(undefined);

    useEffect(() => {
        const authorized = handle(claims);
        setIsAuthorized(authorized);
    }, [claims, handle]);

    // if (isAuthorized === undefined) {
    //     return <Spinner />;
    // }

    // if (!isAuthorized) {
    //     return "Access Denied";
    // }

    return (
        <>
            {isAuthorized === undefined && (
                <Spinner />
            )}
            {!isAuthorized && (
                "Access Denied"
            )}
            {isAuthorized && (
                children
            )}
        </>
    );
};

export default withAuth(Authorize);