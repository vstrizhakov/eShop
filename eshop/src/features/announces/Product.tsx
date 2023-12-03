import React from "react";
import { Card } from "react-bootstrap";

interface IProps {
    name: string,
    url: string,
    price: number,
    discountedPrice?: number,
    currencyName: string,
    description?: string,
};

const Product: React.FC<IProps> = props => {
    const {
        name,
        url,
        price,
        discountedPrice,
        currencyName,
        description,
    } = props;
    return (
        <Card className="bg-body-tertiary border-0 rounded-5 shadow h-100">
            <Card.Body>
                <div>
                    {discountedPrice ? (
                        <span className="text-body-emphasis">{discountedPrice} {currencyName} <s className="text-danger"><small>{price} {currencyName}</small></s></span>
                    ) : (
                        <span className="text-body-emphasis">{price} {currencyName}</span>
                    )}
                </div>
                <a href={url} target="_blank">{name}</a>
                {description && (
                    <p className="m-0">{description}</p>
                )}
            </Card.Body>
        </Card>
    );
};

export default Product;