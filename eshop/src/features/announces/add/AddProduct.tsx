import React from "react";
import { Form as BootstrapForm, Button, Card, Col, InputGroup, Row } from "react-bootstrap";
import { Field, useField } from "react-final-form";
import Check from "../../../components/Check";
import Select from "../../../components/Select";
import TextField from "../../../components/TextField";
import { Currency, useGetCurrenciesQuery, useGetShopsQuery } from "../../api/catalogSlice";

export interface AddProductForm {
    shopId: string,
    name: string,
    url: string,
    price: number,
    discountedPrice?: number,
    currencyId: string,
    sale: boolean,
    discount: number,
    description: string,
};

interface IProps {
    currencies: Currency[],
};

const required = (value: string) => {
    return !value;
};

const validUrl = (value: string) => {
    try {
        new URL(value);
        return undefined;
    } catch (err) {
        return true;
    }
};

function isNumeric(str: string) {
    if (typeof str != "string") return false // we only process strings!  
    return !isNaN(parseFloat(str)) // ...and ensure strings of whitespace fail
}

const number = (value: string) => {
    return !isNumeric(value);
};

const composeValidators = (...validators: ((value: string) => any)[]) => (value: string) =>
    validators.reduce((error, validator) => error || validator(value), undefined);

const AddProduct: React.FC<IProps> = props => {
    const {
        currencies,
    } = props;

    const {
        data: shops,
    } = useGetShopsQuery(undefined);

    const {
        input: {
            value: sale,
        },
    } = useField("sale", {
        subscription: {
            value: true,
        },
    });

    return (
        <Card>
            <Card.Header>
                <strong className="fw-semibold">Додайте позиції</strong>
            </Card.Header>
            <Card.Body>
                {/* <BootstrapForm.FloatingLabel label="Магазин" controlId="product-shop-id" className="mb-3">
                    <Field
                        name="shopId"
                        disabled={shops === undefined}
                        defaultValue={shops && shops.length > 0 && shops[0].id}
                        component={Select}>
                        {shops && shops.map(shop => (
                            <option key={shop.id} value={shop.id}>{shop.name}</option>
                        ))}
                    </Field>
                </BootstrapForm.FloatingLabel> */}

                <Field
                    name="url"
                    validate={composeValidators(required, validUrl)}>
                    {({ input, meta }) => (
                        <BootstrapForm.FloatingLabel label="Посилання" controlId="product-url" className="mb-3">
                            <BootstrapForm.Control
                                {...input}
                                placeholder="Посилання"
                                isInvalid={meta.touched && !!meta.error} />
                            {meta.touched && meta.error && (
                                <BootstrapForm.Control.Feedback type="invalid">Будь ласка, введіть правильне посилання</BootstrapForm.Control.Feedback>
                            )}
                        </BootstrapForm.FloatingLabel>
                    )}
                </Field>

                <Row className="mb-1 flex-wrap-reverse">
                    <Col>
                        <Field
                            name="price"
                            type="number"
                            validate={composeValidators(required, number)}>
                            {({ input, meta }) => (
                                <BootstrapForm.FloatingLabel label="Ціна" controlId="product-price">
                                    <BootstrapForm.Control
                                        {...input}
                                        placeholder="Ціна"
                                        isInvalid={meta.touched && !!meta.error} />
                                    {meta.touched && meta.error && (
                                        <BootstrapForm.Control.Feedback type="invalid">Будь ласка, заповніть це поле</BootstrapForm.Control.Feedback>
                                    )}
                                </BootstrapForm.FloatingLabel>
                            )}
                        </Field>
                    </Col>

                    {sale && (
                        <Col>
                            <Field
                                name="discountedPrice"
                                type="number"
                                validate={composeValidators(required, number)}>
                                {({ input, meta }) => (
                                    <BootstrapForm.FloatingLabel label="Ціна зі знижкою" controlId="product-discounted-price">
                                        <BootstrapForm.Control
                                            {...input}
                                            placeholder="Ціна зі знижкою"
                                            isInvalid={meta.touched && !!meta.error} />
                                        {meta.touched && meta.error && (
                                            <BootstrapForm.Control.Feedback type="invalid">Будь ласка, заповніть це поле</BootstrapForm.Control.Feedback>
                                        )}
                                    </BootstrapForm.FloatingLabel>
                                )}
                            </Field>
                        </Col>
                    )}

                    <Col md="auto" className="mb-3 mb-md-0">
                        <Field
                            name="currencyId"
                            validate={required}>
                            {({ input, meta }) => (
                                <BootstrapForm.FloatingLabel label="Валюта" controlId="product-currency-id">
                                    <BootstrapForm.Select
                                        {...input}
                                        isInvalid={meta.touched && !!meta.error}
                                    >
                                        {currencies && currencies.map(currency => (
                                            <option key={currency.id} value={currency.id}>{currency.name}</option>
                                        ))}
                                    </BootstrapForm.Select>
                                    {meta.touched && meta.error && (
                                        <BootstrapForm.Control.Feedback type="invalid">Будь ласка, заповніть це поле</BootstrapForm.Control.Feedback>
                                    )}
                                </BootstrapForm.FloatingLabel>
                            )}
                        </Field>
                    </Col>
                </Row>

                <Field
                    name="sale"
                    type="checkbox">
                    {({ input, meta }) => (
                        <BootstrapForm.Group controlId="product-sale" className="mb-3">
                            <BootstrapForm.Check
                                {...input}
                                type="checkbox"
                                label="Діє знижка"
                            />
                        </BootstrapForm.Group>
                    )}
                </Field>

                <Field
                    name="name"
                    validate={required}>
                    {({ input, meta }) => (
                        <BootstrapForm.FloatingLabel label="Заголовок" controlId="product-name" className="mb-3">
                            <BootstrapForm.Control
                                {...input}
                                placeholder="Заголовок"
                                isInvalid={meta.touched && !!meta.error} />
                            {meta.touched && meta.error && (
                                <BootstrapForm.Control.Feedback type="invalid">Будь ласка, заповніть це поле</BootstrapForm.Control.Feedback>
                            )}
                        </BootstrapForm.FloatingLabel>
                    )}
                </Field>

                <Field
                    name="description">
                    {({ input, meta }) => (
                        <BootstrapForm.FloatingLabel label="Опис" controlId="product-description" className="mb-3">
                            <BootstrapForm.Control
                                {...input}
                                style={{ height: 100 }}
                                placeholder="Опис"
                                as="textarea" />
                        </BootstrapForm.FloatingLabel>
                    )}
                </Field>

                <Button type="submit" className="fw-semibold">Додати</Button>
            </Card.Body>
        </Card>
    );
};

export default AddProduct;