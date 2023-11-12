import React, { useCallback, useEffect, useMemo, useState } from "react";
import { CheckConfirmationRequest, CheckConfirmationResponse, useCheckConfirmationMutation } from "../api/authSlice";
import { useNavigate, useSearchParams } from "react-router-dom";

const Confirm: React.FC = () => {

    const [searchParams] = useSearchParams();
    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const [checkConfirmation] = useCheckConfirmationMutation();

    const [confirmationInfo, setConfirmationInfo] = useState<CheckConfirmationResponse>();

    const updateConfirmationInfo = useCallback(async () => {
        const request: CheckConfirmationRequest = {
            returnUrl: returnUrl ?? undefined,
        };
        const confirmationInfo = await checkConfirmation(request).unwrap();
        setConfirmationInfo(confirmationInfo);
    }, [checkConfirmation, returnUrl]);

    useEffect(() => {
        updateConfirmationInfo();
    }, [updateConfirmationInfo]);

    const navigate = useNavigate();

    useEffect(() => {
        if (confirmationInfo) {
            if (confirmationInfo.confirmed) {
                if (confirmationInfo.validReturnUrl) {
                    window.location.assign(confirmationInfo.validReturnUrl);
                } else {
                    navigate("/");
                }
            }
        }
    }, [confirmationInfo]);

    return (
        <div>
            {confirmationInfo?.links && (
                <>
                    Telegram: {confirmationInfo.links.telegram}
                    <br />
                    Viber: {confirmationInfo.links.viber}
                </>
            )}
        </div>
    );
};

export default Confirm;