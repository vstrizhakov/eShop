import React, { useCallback, useEffect, useMemo } from "react";
import { Button, Form as BootstrapForm, Row, Col, Anchor } from "react-bootstrap";
import { Form, Field } from "react-final-form";
import { connect, ConnectedProps } from "react-redux";
import { createSearchParams, useHref, useNavigate, useSearchParams } from "react-router-dom";
import { RootState } from "../../app/store";
import TextField from "../../components/TextField";
import { useGetSignInInfoQuery, useSignInMutation } from "../api/authSlice";
import { setIsError } from "./signInSlice";
import { LinkContainer } from "react-router-bootstrap";

const mapStateToProps = (state: RootState) => ({
    isError: state.signIn.isError,
});

const mapDispatchToProps = {
    setIsError,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const SignIn: React.FC<PropsFromRedux> = (props) => {
    const {
        isError,
        setIsError,
    } = props;

    const [searchParams] = useSearchParams();
    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const {
        data: signInInfo,
    } = useGetSignInInfoQuery(undefined);

    useEffect(() => {
        if (signInInfo) {
            if (signInInfo.waitingForConfirmation) {
                navigate(`/auth/confirm?returnUrl=${returnUrl}`);
            }
        }
    }, [signInInfo, returnUrl]);

    const [signIn] = useSignInMutation();

    const navigate = useNavigate();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        if (returnUrl) {
            const response = await signIn({
                phoneNumber: values.phoneNumber,
                password: values.password,
                returnUrl: returnUrl,
            }).unwrap();

            if (response.succeeded) {
                if (response.validReturnUrl) {
                    window.location.assign(response.validReturnUrl);
                }
            } else {
                if (response.confirmationRequired) {
                    const searchParams = createSearchParams({
                        returnUrl,
                    });
                    navigate(`/auth/confirm?${searchParams}`);
                } else {
                    setIsError(true);
                }
            }
        }
    }, [signIn, returnUrl, setIsError]);

    if (!signInInfo || signInInfo.waitingForConfirmation) {
        return <>"Loading..."</>;
    }

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <Row>
                        <Col xs={4}></Col>
                        <Col xs={4}>
                            <h3 className="mb-3">Вхід</h3>

                            <BootstrapForm.Group>
                                <BootstrapForm.Label>Номер телефону</BootstrapForm.Label>
                                <Field
                                    id="phoneNumber"
                                    name="phoneNumber"
                                    type="tel"
                                    pattern="\+380[0-9]{9}"
                                    placeholder="+380123456789"
                                    className="mb-2"
                                    component={TextField} />
                            </BootstrapForm.Group>

                            <BootstrapForm.Group>
                                <BootstrapForm.Label>Пароль</BootstrapForm.Label>
                                <Field
                                    id="password"
                                    name="password"
                                    type="password"
                                    placeholder="Пароль"
                                    className="mb-2"
                                    component={TextField} />
                            </BootstrapForm.Group>

                            {isError && (
                                <center>
                                    <span className="text-danger">Неправильний логін або пароль</span>
                                </center>
                            )}
                            <div className="d-flex flex-column mt-2">
                                <Button
                                    className="fw-semibold"
                                    type="submit"
                                    variant="primary">
                                    Увійти
                                </Button>
                                <LinkContainer to={{
                                    pathname: "/auth/signUp",
                                    search: searchParams.toString(),
                                }}>
                                    <Anchor className="align-self-center text-decoration-none mt-1">
                                        Зареєструватися
                                    </Anchor>
                                </LinkContainer>
                            </div>
                        </Col>
                    </Row>
                </BootstrapForm >
            )}
        />
    );
};

export default connector(SignIn);