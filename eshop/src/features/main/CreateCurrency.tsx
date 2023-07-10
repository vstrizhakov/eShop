import React, { useCallback } from "react";
import { CreateCurrencyRequest, useCreateCurrencyMutation } from "../api/catalogSlice";
import { Field, Form } from "react-final-form";
import { Form as BootstrapForm, Button } from "react-bootstrap";
import TextField from "../../components/TextField";

const CreateCurrency: React.FC = () => {
    const [createCurrency] = useCreateCurrencyMutation();

    const onSubmit = useCallback((values: Record<string, any>) => {
        const request: CreateCurrencyRequest = {
            name: values.name,
        };
        createCurrency(request);
    }, []);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm onSubmit={handleSubmit}>
                    <h2>Створити Валюту</h2>

                    <Field
                        id="name"
                        name="name"
                        label="Name"
                        type="text"
                        component={TextField} />

                    <Button
                        type="submit"
                        variant="primary">
                        Створити
                    </Button>
                </BootstrapForm>
            )}
        />
    );
};

export default CreateCurrency;