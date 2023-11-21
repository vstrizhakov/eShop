import React, { useCallback, useEffect, useMemo } from "react";
import { Row, Col, Button, Form as BootstrapForm, Anchor } from "react-bootstrap";
import { Form, Field } from "react-final-form";
import { useNavigate, useSearchParams } from "react-router-dom";
import TextField from "../../components/TextField";
import { CompletePasswordResetRequest, useCompletePasswordResetMutation } from "../api/authSlice";
import { RootState } from "../../app/store";
import { setError } from "./completePasswordResetSlice";
import { ConnectedProps, connect } from "react-redux";
import { LinkContainer } from "react-router-bootstrap";
import LoadingButton from "../../components/LoadingButton";
import { ErrorCode } from "../api/apiSlice";

const mapStateToProps = (state: RootState) => ({
    error: state.completePasswordReset.error,
});

const mapDispatchToProps = {
    setError,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const CompletePasswordReset: React.FC<PropsFromRedux> = props => {
    const {
        error,
        setError,
    } = props;

    const [searchParams] = useSearchParams();
    const phoneNumber = useMemo(() => searchParams.get("phoneNumber"), [searchParams]);
    const token = useMemo(() => searchParams.get("token"), [searchParams]);

    const navigate = useNavigate();

    useEffect(() => {
        if (!phoneNumber || !token) {
            navigate("/auth/signIn");
        }
    }, [phoneNumber, token]);

    const [completePasswordReset, {
        isLoading,
    }] = useCompletePasswordResetMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        const request: CompletePasswordResetRequest = {
            phoneNumber: values.phoneNumber,
            token: token!,
            newPassword: values.newPassword,
        };

        const response = await completePasswordReset(request).unwrap();
        if (response.isSuccess) {
            navigate("/auth/signIn");
        } else {
            setError(response.errorCode!);
        }
    }, []);

    return (
        <Form
            initialValues={{
                phoneNumber: phoneNumber,
            }}
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <Row>
                        <Col xs={4}></Col>
                        <Col xs={4}>
                            <h2 className="mb-3">Відновлення паролю</h2>

                            <Field
                                id="phoneNumber"
                                name="phoneNumber"
                                type="tel"
                                pattern="\+380[0-9]{9}"
                                placeholder="Номер телефону"
                                className="mb-2"
                                disabled={true}
                                component={TextField}
                            />
                            <Field
                                id="newPassword"
                                name="newPassword"
                                type="password"
                                placeholder="Новий пароль"
                                className="mb-2"
                                component={TextField}
                                required={true}
                            />
                            <BootstrapForm.Text>
                                <span>Пароль має відповідати наступним вимогам:</span>
                                <ul className="m-0">
                                    <li>Бути не менш ніж 6 символів</li>
                                    <li>Містити хоча б одну маленьку букву</li>
                                    <li>Містити хоча б одну велику букву</li>
                                    <li>Містити хоча б одну цифру</li>
                                </ul>
                            </BootstrapForm.Text>

                            <div className="d-flex flex-column mt-2">
                                <LoadingButton
                                    type="submit"
                                    className="mt-2 fw-semibold"
                                    variant="primary"
                                    isLoading={isLoading}>
                                    Продовжити
                                </LoadingButton>
                            </div>

                            {error && (
                                <BootstrapForm.Text className="d-flex flex-column text-danger text-center">
                                    {error === ErrorCode.InvalidPassword && (
                                        <span>Введений пароль не відповідає вимогам.</span>
                                    )}
                                    {error === ErrorCode.UserNotFound && (
                                        <>
                                            <span>Користувача із вказаним номером телефону не знайдено</span>
                                            <LinkContainer to="/auth/requestPasswordReset">
                                                <Anchor>Спробуйте спочатку</Anchor>
                                            </LinkContainer>
                                        </>
                                    )}
                                </BootstrapForm.Text>
                            )}
                        </Col>
                    </Row>
                </BootstrapForm>
            )}
        />
    );
};

export default connector(CompletePasswordReset);