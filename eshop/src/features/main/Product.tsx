import React from "react";
import { CreateProductRequest, useGetCurrenciesQuery } from "../api/catalogSlice";
import { Card } from "react-bootstrap";

interface IProps {
    product: CreateProductRequest,
}

const Product: React.FC<IProps> = props => {
    const {
        product: {
            name,
            url,
            price,
        },
    } = props;

    const {
        data: currencies,
    } = useGetCurrenciesQuery(undefined);

    const currency = currencies?.find(e => e.id == price.currencyId);

    return (
        <Card>
            <Card.Body>
                <a href={url} target="_blank">{name}</a>
                <div>{price.price} {currency?.name}</div>
            </Card.Body>
        </Card>
    );
};

export default Product;