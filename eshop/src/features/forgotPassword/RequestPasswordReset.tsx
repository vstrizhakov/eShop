import React, { useCallback, useEffect } from "react";
import { Row, Col, Button, Anchor, Form as BootstrapForm } from "react-bootstrap";
import { Field, Form } from "react-final-form";
import { LinkContainer } from "react-router-bootstrap";
import TextField from "../../components/TextField";
import { useSearchParams } from "react-router-dom";
import { RequestPasswordResetRequest, useRequestPasswordResetMutation } from "../api/authSlice";
import LoadingButton from "../../components/LoadingButton";
import { RootState } from "../../app/store";
import { ConnectedProps, connect } from "react-redux";
import { setResult, reset } from "./requestPasswordResetSlice";
import { ErrorCode } from "../api/apiSlice";

const mapStateToProps = (state: RootState) => ({
    result: state.requestPasswordReset.result,
});

const mapDispatchToProps = {
    setResult,
    reset,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

const RequestPasswordReset: React.FC<PropsFromRedux> = props => {
    const {
        result,
        setResult,
        reset,
    } = props;

    const [searchParams] = useSearchParams();

    const [requestPasswordReset, {
        isLoading,
    }] = useRequestPasswordResetMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        const request: RequestPasswordResetRequest = {
            phoneNumber: values.phoneNumber,
        };

        const response = await requestPasswordReset(request).unwrap();
        setResult({
            isRequested: response.succeeded,
            error: response.errorCode,
        });
    }, [requestPasswordReset]);

    useEffect(() => {
        return () => {
            reset();
        };
    }, [reset]);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <Row>
                        <Col md={{offset: 2, span: 8}} lg={{offset: 3, span: 6}} xxl={{offset: 4, span: 4}}>
                            <h2 className="mb-3">Відновлення паролю</h2>

                            <Field
                                id="phoneNumber"
                                name="phoneNumber"
                                type="tel"
                                pattern="\+380[0-9]{9}"
                                placeholder="Номер телефону"
                                className="mb-2"
                                required={true}
                                disabled={result?.isRequested}
                                component={TextField}
                            />

                            <div className="d-flex flex-column mt-2">
                                <LoadingButton
                                    type="submit"
                                    className="mt-2 fw-semibold"
                                    variant="primary"
                                    isLoading={isLoading}
                                    disabled={result?.isRequested}>
                                    Продовжити
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

                            {result?.isRequested && (
                                <div className="lh-1 text-center mt-2">
                                    <BootstrapForm.Text className="text-success">Посилання на відновлення паролю було відправлено у месенджери, прив'язані до вказаного номера</BootstrapForm.Text>
                                </div>
                            )}
                            {result?.error === ErrorCode.UserNotFound && (
                                <BootstrapForm.Text className="text-danger">Користувача із вказаним номером телефону не знайдено</BootstrapForm.Text>
                            )}
                        </Col>
                    </Row>
                </BootstrapForm>
            )}
        />
    );
};

export default connector(RequestPasswordReset);