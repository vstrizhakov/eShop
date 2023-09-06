import React, { useCallback, useMemo } from "react";
import { Button, Form as BootstrapForm } from "react-bootstrap";
import { Field, Form } from "react-final-form";
import { useNavigate, useSearchParams } from "react-router-dom";
import TextField from "../../components/TextField";
import { useSignUpMutation } from "../api/authSlice";

const SignUp: React.FC = () => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const returnUrl = useMemo(() => searchParams.get("returnUrl"), [searchParams]);

    const [signUp] = useSignUpMutation();

    const onSubmit = useCallback(async (values: Record<string, any>) => {
        const response = await signUp({
            firstName: values.firstName,
            lastName: values.lastName,
            email: values.email,
            phoneNumber: values.phoneNumber,
            password: values.password,
        }).unwrap();

        if (response.succeeded) {
            let url = "/auth/signIn";
            if (returnUrl) {
                url += `?returnUrl=${returnUrl}`;
            }
            navigate(url);
        }
    }, [signUp, navigate]);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <h2>Sign Up</h2>

                    <Field
                        id="firstName"
                        name="firstName"
                        type="text"
                        placeholder="First Name"
                        component={TextField} />
                    <Field
                        id="lastName"
                        name="lastName"
                        type="text"
                        placeholder="Last Name"
                        component={TextField} />
                    <Field
                        id="email"
                        name="email"
                        type="email"
                        placeholder="Email"
                        component={TextField} />
                    <Field
                        id="phoneNumber"
                        name="phoneNumber"
                        type="text"
                        placeholder="Phone Number"
                        component={TextField} />
                    <Field
                        id="password"
                        name="password"
                        type="password"
                        placeholder="Password"
                        component={TextField} />
                    <div>
                        <Button
                            type="submit"
                            variant="primary">
                            Зареєструватися
                        </Button>
                    </div>
                </BootstrapForm>
            )}
        />
    );
};

export default SignUp;