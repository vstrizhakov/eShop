import React, { useCallback, useEffect, useMemo } from "react";
import { Row, Col, Button, Form as BootstrapForm, Anchor } from "react-bootstrap";
import { Form, Field } from "react-final-form";
import { useNavigate, useSearchParams } from "react-router-dom";
import TextField from "../../components/TextField";
import { CompletePasswordResetRequest, useCompletePasswordResetMutation } from "../api/authSlice";
import { RootState } from "../../app/store";
import { setIsError } from "./completePasswordResetSlice";
import { ConnectedProps, connect } from "react-redux";
import { LinkContainer } from "react-router-bootstrap";

const mapStateToProps = (state: RootState) => ({
    isError: state.completePasswordReset.isError,
});

const mapDispatchToProps = {
    setIsError,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const CompletePasswordReset: React.FC<PropsFromRedux> = props => {
    const {
        isError,
        setIsError,
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

    const [completePasswordReset] = useCompletePasswordResetMutation();

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
            setIsError(true);
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
                                component={TextField} />
                            <Field
                                id="newPassword"
                                name="newPassword"
                                type="password"
                                placeholder="Новий пароль"
                                className="mb-2"
                                component={TextField} />

                            {isError && (
                                <center>
                                    <span className="text-danger">Сталася помилка під час відновлення паролю. <LinkContainer className="text-decoration-none" to="/auth/requestPasswordReset">
                                        <Anchor>Спробуйте ще раз.</Anchor>
                                    </LinkContainer></span>
                                    
                                </center>
                            )}

                            <div className="d-flex flex-column mt-2">
                                <Button
                                    type="submit"
                                    className="mt-2 fw-semibold"
                                    variant="primary">
                                    Продовжити
                                </Button>
                            </div>
                        </Col>
                    </Row>
                </BootstrapForm>
            )}
        />
    );
};

export default connector(CompletePasswordReset);