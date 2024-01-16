import React, { PropsWithChildren, useEffect, useState } from "react";
import { Spinner } from "react-bootstrap";
import { AuthContextProps } from "./authContext";
import { Claims } from "./authSlice";
import { withAuth } from "./withAuth";
import { useNavigate } from "react-router-dom";

interface IProps extends AuthContextProps {
    handle?: (claims: Claims) => boolean,
};

const Authorize: React.FC<PropsWithChildren<IProps>> = props => {
    const {
        claims,
        handle,
        children,
    } = props;

    const [isAuthorized, setIsAuthorized] = useState<boolean | undefined>();

    useEffect(() => {
        if (claims) {
            const authorized = handle ? handle(claims) : true;
            setIsAuthorized(authorized);
        } else {
            setIsAuthorized(false);
        }
    }, [claims, handle]);

    const navigate = useNavigate();

    useEffect(() => {
        if (isAuthorized === false) {
            navigate("/");
        }
    }, [isAuthorized]);

    return (
        <>
            {!isAuthorized && (
                <Spinner />
            )}
            {isAuthorized && (
                children
            )}
        </>
    );
};

export default withAuth(Authorize);