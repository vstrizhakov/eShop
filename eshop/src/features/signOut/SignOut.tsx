import React, { useEffect, useMemo } from "react";
import { useSearchParams } from "react-router-dom";
import { useSignOutMutation, useTrySignOutQuery } from "../api/authSlice";

const SignOut: React.FC = () => {
    const [searchParams] = useSearchParams();

    const logoutId = useMemo(() => searchParams.get("logoutId"), [searchParams]);

    const {
        data: signOutInfo,
    } = useTrySignOutQuery(logoutId!); // check for null

    const [signOut] = useSignOutMutation();

    useEffect(() => {
        if (signOutInfo) {
            if (!signOutInfo.prompt && signOutInfo.postInfo?.redirectUrl) {
                window.location.replace(signOutInfo.postInfo.redirectUrl);
            }
        }
    }, [signOutInfo, signOut]);

    return (
        <div>Signing you out...</div>
    );
};

export default SignOut;