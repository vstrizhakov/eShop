import React, { InputHTMLAttributes, useCallback } from "react";
import { Field, Form } from "react-final-form";
import { Form as BootstrapForm, Button, Col, Row } from "react-bootstrap";
import TextField from "../../components/TextField";
import FileField from "../../components/FileField";
import { CreateCompositionRequest, useCreateCompositionMutation, useGetCurrenciesQuery } from "../api/catalogSlice";
import Select from "../../components/Select";

const CreateComposition: React.FC = () => {
    const [createComposition] = useCreateCompositionMutation();

    const {
        data: currencies,
    } = useGetCurrenciesQuery(undefined);

    const onSubmit = useCallback((values: Record<string, any>) => {
        const request: CreateCompositionRequest = {
            image: values.image,
            products: values.products,
        };
        createComposition(request);
    }, [createComposition]);

    return (
        <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
                <BootstrapForm
                    onSubmit={handleSubmit}
                    encType="multipart/form-data">
                    <h2>Створити Композицію</h2>

                    <Field
                        id="image"
                        name="image"
                        label="Image"
                        type="file"
                        component={FileField} />

                    <Field
                        id="products[0].name"
                        name="products[0].name"
                        type="text"
                        label="Product Name"
                        component={TextField} />

                    <Field
                        id="products[0].url"
                        name="products[0].url"
                        type="text"
                        label="Product Url"
                        component={TextField} />

                    <Row>
                        <Col>
                            <Field
                                id="products[0].price.price"
                                name="products[0].price.price"
                                type="text"
                                label="Product Price"
                                component={TextField} />
                        </Col>
                        <Col>
                            <Field
                                id="products[0].price.currencyId"
                                name="products[0].price.currencyId"
                                label="Product Price Currency"
                                defaultValue={currencies && currencies[0].id}
                                component={Select}>
                                {currencies && currencies.map(currency => (
                                    <option key={currency.id} value={currency.id}>{currency.name}</option>
                                ))}
                            </Field>
                        </Col>
                    </Row>

                    <Button
                        type="submit"
                        variant="primary">
                        Створити
                    </Button>
                </BootstrapForm>
            )} />
    );
};

export default CreateComposition;