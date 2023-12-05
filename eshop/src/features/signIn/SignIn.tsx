import React, { useCallback, useEffect, useMemo } from "react";
import { Form as BootstrapForm, Row, Col, Anchor, Spinner } from "react-bootstrap";
import { Form, Field } from "react-final-form";
import { connect, ConnectedProps } from "react-redux";
import { useNavigate, useSearchParams } from "react-router-dom";
import { RootState } from "../../app/store";
import TextField from "../../components/TextField";
import { useGetSignInInfoQuery, useSignInMutation } from "../api/authSlice";
import { setIsError, savePhoneNumberForConfirmation } from "./signInSlice";
import { LinkContainer } from "react-router-bootstrap";
import LoadingButton from "../../components/LoadingButton";

const mapStateToProps = (state: RootState) => ({
    isError: state.signIn.isError,
});

const mapDispatchToProps = {
    setIsError,
    savePhoneNumberForConfirmation,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const SignIn: React.FC<PropsFromRedux> = props => {
    const {
        isError,
        setIsError,
        savePhoneNumberForConfirmation,
    } = props;

    const [searchParams] = useSearchParams();
    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const {
        data: signInInfo,
    } = useGetSignInInfoQuery(undefined);

    const navigate = useNavigate();

    useEffect(() => {
        if (signInInfo) {
            if (signInInfo.waitingForConfirmation) {
                navigate(`/auth/confirm?${searchParams}`);
            }
        }
    }, [signInInfo, navigate, searchParams]);

    const [signIn, {
        isLoading,
    }] = useSignInMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        const response = await signIn({
            phoneNumber: values.phoneNumber,
            password: values.password,
            returnUrl: returnUrl ?? undefined,
        }).unwrap();

        if (response.succeeded) {
            if (response.validReturnUrl) {
                window.location.assign(response.validReturnUrl);
            }
        } else {
            if (response.confirmationRequired) {
                savePhoneNumberForConfirmation(values.phoneNumber);
                navigate(`/auth/confirm?${searchParams}`);
            } else {
                setIsError(true);
            }
        }
    }, [signIn, returnUrl, setIsError, navigate, savePhoneNumberForConfirmation, searchParams]);

    if (!signInInfo || signInInfo.waitingForConfirmation) {
        return (
            <div className="d-flex justify-content-center">
                <Spinner />
            </div>
        );
    }

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <Row>
                        <Col md={{ offset: 2, span: 8 }} lg={{ offset: 3, span: 6 }} xxl={{ offset: 4, span: 4 }}>
                            <h3 className="mb-3">Вхід</h3>

                            <Field
                                id="phoneNumber"
                                name="phoneNumber"
                                type="tel"
                                pattern="\+380[0-9]{9}"
                                placeholder="Номер телефону"
                                className="mb-2"
                                component={TextField}
                                required={true}
                            />

                            <Field
                                id="password"
                                name="password"
                                type="password"
                                placeholder="Пароль"
                                className="mb-2"
                                component={TextField}
                                required={true}
                            />

                            {isError && (
                                <center>
                                    <span className="text-danger">Неправильний логін або пароль</span>
                                </center>
                            )}

                            <div className="d-flex flex-column mt-2">
                                <LoadingButton
                                    className="fw-semibold"
                                    type="submit"
                                    variant="primary"
                                    isLoading={isLoading}>
                                    Увійти
                                </LoadingButton>
                                <div className="d-flex justify-content-between">
                                    <LinkContainer to={{
                                        pathname: "/auth/signUp",
                                        search: searchParams.toString(),
                                    }}>
                                        <Anchor className="text-muted text-decoration-none mt-1">
                                            Зареєструватись
                                        </Anchor>
                                    </LinkContainer>

                                    <LinkContainer to={{
                                        pathname: "/auth/requestPasswordReset",
                                        search: searchParams.toString(),
                                    }}>
                                        <Anchor className="text-muted text-decoration-none mt-1">
                                            Забули пароль?
                                        </Anchor>
                                    </LinkContainer>
                                </div>
                            </div>
                        </Col>
                    </Row>
                </BootstrapForm >
            )}
        />
    );
};

export default connector(SignIn);