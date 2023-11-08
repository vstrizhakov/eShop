import React, { useCallback, useMemo } from "react";
import { Button, Form as BootstrapForm, Row, Col, Anchor } from "react-bootstrap";
import { Field, Form } from "react-final-form";
import { useNavigate, useSearchParams } from "react-router-dom";
import TextField from "../../components/TextField";
import { useSignUpMutation } from "../api/authSlice";
import { LinkContainer } from "react-router-bootstrap";

const SignUp: React.FC = () => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const [signUp] = useSignUpMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        if (returnUrl) {
            const response = await signUp({
                firstName: values.firstName,
                lastName: values.lastName,
                phoneNumber: values.phoneNumber,
                password: values.password,
            }).unwrap();

            if (response.succeeded) {
                navigate(returnUrl);
            }
        }
    }, [signUp, navigate, returnUrl]);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <Row>
                        <Col xs={4}></Col>
                        <Col xs={4}>
                            <h2 className="mb-3">Реєстрація</h2>

                            <Row>
                                <Col>
                                    <Field
                                        id="firstName"
                                        name="firstName"
                                        type="text"
                                        placeholder="Ім'я"
                                        className="mb-2"
                                        component={TextField} />
                                </Col>
                                <Col>
                                    <Field
                                        id="lastName"
                                        name="lastName"
                                        type="text"
                                        placeholder="Прізвище"
                                        className="mb-2"
                                        component={TextField} />
                                </Col>
                            </Row>
                            <Field
                                id="phoneNumber"
                                name="phoneNumber"
                                type="tel"
                                placeholder="Номер телефону"
                                className="mb-2"
                                component={TextField} />
                            <Field
                                id="phoneNumber"
                                name="phoneNumber"
                                type="text"
                                placeholder="Номер телефону"
                                className="mb-2"
                                component={TextField} />
                            <Field
                                id="password"
                                name="password"
                                type="password"
                                placeholder="Пароль"
                                className="mb-2"
                                component={TextField} />


                            <div className="d-flex flex-column mt-2">
                                <Button
                                    type="submit"
                                    className="mt-2 fw-semibold"
                                    variant="primary">
                                    Зареєструватися
                                </Button>
                                <LinkContainer to={{
                                    pathname: "/auth/signIn",
                                    search: searchParams.toString(),
                                }}>
                                    <Anchor className="align-self-center text-decoration-none mt-1">
                                        Увійти
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

export default SignUp;