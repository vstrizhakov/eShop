import React, { useCallback, useEffect } from "react";
import { Form as BootstrapForm, Row, Col, Anchor } from "react-bootstrap";
import { Field, Form } from "react-final-form";
import { useNavigate, useSearchParams } from "react-router-dom";
import TextField from "../../components/TextField";
import { useSignUpMutation } from "../api/authSlice";
import { LinkContainer } from "react-router-bootstrap";
import { RootState } from "../../app/store";
import { setError, reset, savePhoneNumberForConfirmation } from "./signUpSlice";
import { ConnectedProps, connect } from "react-redux";
import { ErrorCode } from "../api/apiSlice";
import LoadingButton from "../../components/LoadingButton";

const mapStateToProps = (state: RootState) => ({
    error: state.signUp.error,
});

const mapDispatchToProps = {
    setError,
    reset,
    savePhoneNumberForConfirmation,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const SignUp: React.FC<PropsFromRedux> = props => {
    const {
        error,
        setError,
        reset,
        savePhoneNumberForConfirmation,
    } = props;

    useEffect(() => {
        return () => {
            reset();
        };
    }, []);

    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const [signUp, {
        isLoading,
    }] = useSignUpMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        const response = await signUp({
            firstName: values.firstName,
            lastName: values.lastName,
            phoneNumber: values.phoneNumber,
            password: values.password,
        }).unwrap();

        if (response.succeeded) {
            savePhoneNumberForConfirmation(values.phoneNumber);
            navigate(`/auth/confirm?${searchParams}`);
        } else {
            switch (response.errorCode) {
                case ErrorCode.UserAlreadyExists:
                    setError("Користувач із вказаним номером вже зареєстрований.");
                    break;
                case ErrorCode.InvalidPassword:
                    setError("Введений пароль не відповідає вимогам.");
                    break;
            }
        }
    }, [signUp, navigate, setError]);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <Row>
                        <Col xs={4}></Col>
                        <Col xs={4}>
                            <h2 className="mb-3">Реєстрація</h2>

                            <Row className="mb-2">
                                <Col>
                                    <Field
                                        id="firstName"
                                        name="firstName"
                                        type="text"
                                        placeholder="Ім'я"
                                        required={true}
                                        component={TextField} />
                                </Col>
                                <Col>
                                    <Field
                                        id="lastName"
                                        name="lastName"
                                        type="text"
                                        placeholder="Прізвище"
                                        required={true}
                                        component={TextField} />
                                </Col>
                            </Row>

                            <Field
                                id="phoneNumber"
                                name="phoneNumber"
                                type="tel"
                                pattern="\+380[0-9]{9}"
                                placeholder="Номер телефону"
                                className="mb-2"
                                required={true}
                                component={TextField} />
                            <BootstrapForm.Group className="mb-2">
                                <Field
                                    id="password"
                                    name="password"
                                    type="password"
                                    placeholder="Пароль"
                                    required={true}
                                    component={TextField} />
                                <div className="form-text">
                                    Пароль має відповідати наступним вимогам:
                                    <ul className="m-0">
                                        <li>Бути не менш ніж 6 символів</li>
                                        <li>Містити хоча б одну маленьку букву</li>
                                        <li>Містити хоча б одну велику букву</li>
                                        <li>Містити хоча б одну цифру</li>
                                    </ul>
                                </div>
                            </BootstrapForm.Group>

                            <div className="d-flex flex-column mt-2">
                                <LoadingButton
                                    type="submit"
                                    className="mt-2 fw-semibold"
                                    variant="primary"
                                    isLoading={isLoading}>
                                    Зареєструватися
                                </LoadingButton>
                                <LinkContainer to={{
                                    pathname: "/auth/signIn",
                                    search: searchParams.toString(),
                                }}>
                                    <Anchor className="text-muted align-self-center text-decoration-none mt-1">
                                        Увійти
                                    </Anchor>
                                </LinkContainer>
                            </div>

                            {error && (
                                <BootstrapForm.Text className="d-block text-danger text-center">{error}</BootstrapForm.Text>
                            )}
                        </Col>
                    </Row>
                </BootstrapForm>
            )}
        />
    );
};

export default connector(SignUp);