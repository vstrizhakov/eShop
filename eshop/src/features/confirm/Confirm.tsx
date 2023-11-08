import React, { useCallback, useEffect, useMemo, useState } from "react";
import { CheckConfirmationResponse, useCheckConfirmationMutation } from "../api/authSlice";
import { useNavigate, useSearchParams } from "react-router-dom";

const Confirm: React.FC = () => {
    
    const [searchParams] = useSearchParams();
    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const [checkConfirmation] = useCheckConfirmationMutation();

    const [confirmationInfo, setConfirmationInfo] = useState<CheckConfirmationResponse>();

    const updateConfirmationInfo = useCallback(async () => {
        const confirmationInfo = await checkConfirmation(undefined).unwrap();
        setConfirmationInfo(confirmationInfo);
    }, [checkConfirmation]);

    useEffect(() => {
        updateConfirmationInfo();
    }, [updateConfirmationInfo]);

    const confirmed = confirmationInfo?.confirmed;
    useEffect(() => {
        if (confirmed && returnUrl) {
            window.location.assign(returnUrl);
        } 
    }, [confirmed]);

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