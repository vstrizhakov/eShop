import React, { useCallback } from "react";
import { Row, Col, Button, Anchor, Form as BootstrapForm } from "react-bootstrap";
import { Field, Form } from "react-final-form";
import { LinkContainer } from "react-router-bootstrap";
import TextField from "../../components/TextField";
import { useSearchParams } from "react-router-dom";
import { RequestPasswordResetRequest, useRequestPasswordResetMutation } from "../api/authSlice";

const RequestPasswordReset: React.FC = () => {
    const [searchParams] = useSearchParams();

    const [requestPasswordReset] = useRequestPasswordResetMutation();

    const onSubmit = useCallback((values: Record<string, any>) => {
        const request: RequestPasswordResetRequest = {
            phoneNumber: values.phoneNumber,
        };
        
        requestPasswordReset(request);
    }, [requestPasswordReset]);

    return (
        <Form
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
                                component={TextField} />

                            <div className="d-flex flex-column mt-2">
                                <Button
                                    type="submit"
                                    className="mt-2 fw-semibold"
                                    variant="primary">
                                    Продовжити
                                </Button>
                                <LinkContainer to={{
                                    pathname: "/auth/signIn",
                                    search: searchParams.toString(),
                                }}>
                                    <Anchor className="text-muted align-self-center text-decoration-none mt-1">
                                        Увійти
                                    </Anchor>
                                </LinkContainer>
                            </div>
                        </Col>
                    </Row>
                </BootstrapForm>
            )}
        />
    );
};

export default RequestPasswordReset;