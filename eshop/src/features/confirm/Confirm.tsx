import React, { useCallback, useEffect, useMemo } from "react";
import { CheckConfirmationRequest, useCancelConfirmationMutation, useCheckConfirmationMutation } from "../api/authSlice";
import { createSearchParams, useNavigate, useSearchParams } from "react-router-dom";
import { RootState } from "../../app/store";
import { setLinks, reset, readPhoneNumber } from "./confirmSlice";
import { ConnectedProps, connect } from "react-redux";
import { Anchor, Button, Col, Row, Form, Spinner } from "react-bootstrap";
import { isFetchBaseQueryError } from "../../services/helpers";
import { ReactComponent as ViberIcon } from "../../assets/viber.svg";
import { ReactComponent as TelegramIcon } from "../../assets/telegram.svg";
import LoadingButton from "../../components/LoadingButton";

const mapStateToProps = (state: RootState) => ({
    phoneNumber: state.confirm.phoneNumber,
    links: state.confirm.links,
});

const mapDispatchToProps = {
    readPhoneNumber,
    setLinks,
    reset,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const Confirm: React.FC<PropsFromRedux> = props => {
    const {
        readPhoneNumber,
        phoneNumber,
        links,
        setLinks,
        reset,
    } = props;

    useEffect(() => {
        readPhoneNumber();

        return () => {
            reset();
        };
    }, []);

    const [searchParams] = useSearchParams();
    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const [checkConfirmation, {
        isLoading,
    }] = useCheckConfirmationMutation();

    const navigate = useNavigate();

    const updateConfirmationInfo = useCallback(async () => {
        const request: CheckConfirmationRequest = {
            returnUrl: returnUrl ?? undefined,
        };
        try {
            const confirmationInfo = await checkConfirmation(request).unwrap();
            if (confirmationInfo.confirmed) {
                if (confirmationInfo.validReturnUrl) {
                    window.location.assign(confirmationInfo.validReturnUrl);
                } else {
                    navigate("/");
                }
            } else {
                setLinks(confirmationInfo.links!);
            }
        } catch (error: any) {
            if (isFetchBaseQueryError(error)) {
                if (error.status === 400) {
                    navigate(`/auth/signIn?${searchParams}`);
                }
            }
        }
    }, [checkConfirmation, returnUrl, setLinks]);

    useEffect(() => {
        updateConfirmationInfo();
    }, [updateConfirmationInfo]);

    const [cancelConfirmation] = useCancelConfirmationMutation();

    const onCancel = useCallback(async () => {
        await cancelConfirmation(undefined);
        navigate(`/auth/signIn?${searchParams}`);
    }, [cancelConfirmation]);

    const onContinue = useCallback(() => {
        updateConfirmationInfo();
    }, [updateConfirmationInfo]);

    if (!links) {
        return <div>Завантаження...</div>;
    }

    return (
        <Row>
            <Col xs={4}></Col>
            <Col xs={4}>
                <div className="vstack mb-3">
                    <Form.Group>
                        <Form.Control
                            value={phoneNumber}
                            disabled={true} />
                        <Form.Text className="d-flex gap-1">
                            <span>Неправильний номер телефону?</span>
                            <Anchor className="text-muted align-self-center" onClick={onCancel}>
                                Змінити
                            </Anchor>
                        </Form.Text>
                    </Form.Group>

                    <LoadingButton
                        className="fw-semibold mt-2"
                        type="submit"
                        variant="primary"
                        onClick={onContinue}
                        isLoading={isLoading}>
                        <span>Продовжити</span>
                    </LoadingButton>
                </div>

                <div className="vstack gap-3">
                    <span className="text-center">Щоб продовжити, підтвердіть ваш номер телефону за допомогою одного з месенджерів:</span>
                    <div className="hstack gap-3 justify-content-center">
                        <Button variant="outline-primary" className="d-flex gap-1 fw-semibold" href={links.telegram} target="_blank">
                            <TelegramIcon fill="currentColor" height="24px" width="24px" />
                            <span>Telegram</span>
                        </Button>
                        <Button variant="outline-primary" className="d-flex gap-1 fw-semibold" href={links.viber} target="_blank">
                            <ViberIcon fill="currentColor" height="24px" width="24px" />
                            <span>Viber</span>
                        </Button>
                    </div>
                </div>
            </Col>
        </Row>
    );
};

export default connector(Confirm);