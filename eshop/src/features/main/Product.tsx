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
            price: {
                price,
                discountedPrice,
                currencyId,
            },
            description,
        },
    } = props;

    const {
        data: currencies,
    } = useGetCurrenciesQuery(undefined);

    const currency = currencies?.find(e => e.id == currencyId);

    return (
        <Card className="bg-body-tertiary border-0 rounded-5 shadow h-100">
            <Card.Body>
                <div>
                    {discountedPrice ? (
                        <span className="text-body-emphasis">{discountedPrice} {currency?.name} <s className="text-danger"><small>{price} {currency?.name}</small></s></span>
                    ) : (
                        <span className="text-body-emphasis">{price} {currency?.name}</span>
                    )}
                </div>
                <a href={url} target="_blank">{name}</a>
                <p className="m-0">{description}</p>
            </Card.Body>
        </Card>
    );
};

export default Product;