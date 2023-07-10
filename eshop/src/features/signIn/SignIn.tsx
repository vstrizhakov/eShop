import React, { useCallback, useMemo } from "react";
import { Button, Form as BootstrapForm } from "react-bootstrap";
import { Form, Field } from "react-final-form";
import { connect, ConnectedProps } from "react-redux";
import { useNavigate, useSearchParams } from "react-router-dom";
import { RootState } from "../../app/store";
import TextField from "../../components/TextField";
import { useSignInMutation } from "../api/authSlice";
import { setIsError } from "./signInSlice";

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

    const [signIn] = useSignInMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        if (returnUrl) {
            const response = await signIn({
                username: values.username,
                password: values.password,
                returnUrl: returnUrl,
            }).unwrap();

            if (response.succeeded) {
                if (response.validReturnUrl) {
                    window.location.replace(response.validReturnUrl);
                }
            } else {
                setIsError(true);
            }
        }
    }, [signIn, returnUrl, setIsError]);

    const navigate = useNavigate();

    const signUp = useCallback(() => {
        navigate("/auth/signUp");
    }, [navigate]);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <h2>Sign In</h2>

                    <Field
                        id="username"
                        name="username"
                        type="text"
                        label="Username"
                        component={TextField} />
                    <Field
                        id="password"
                        name="password"
                        type="password"
                        label="Password"
                        component={TextField} />
                    {isError && (
                        <div>Неправильний логін або пароль</div>
                    )}
                    <div>
                        <Button
                            type="submit"
                            variant="primary">
                            Увійти
                        </Button>
                        <Button
                            type="button"
                            onClick={signUp}>
                            Зареєструватися
                        </Button>
                    </div>
                </BootstrapForm>
            )}
        />
    );
};

export default connector(SignIn);