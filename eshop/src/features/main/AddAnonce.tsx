import React, { useCallback, useState } from "react";
import { Col, Row, Form as BootstrapForm, Button } from "react-bootstrap";
import PickImageStep, { ImageFormValues } from "./PickImageStep";
import SelectShopStep, { ShopFormValues } from "./SelectShopStep";
import AddProduct, { AddProductForm } from "./AddProduct";
import { Form } from "react-final-form";
import { CreateCompositionRequest, CreateProductRequest, useCreateCompositionMutation } from "../api/catalogSlice";
import Product from "./Product";

const AddAnnounce: React.FC = () => {
    const [image, setImage] = useState<File | undefined>(undefined);
    const [shopId, setShopId] = useState<string | undefined>(undefined);
    const [products, setProducts] = useState<CreateProductRequest[]>([]);

    const onImageFormSubmit = useCallback((values: ImageFormValues) => {
        setImage(values.image);
    }, [setImage]);

    const onShopFormSubmit = useCallback((values: ShopFormValues) => {
        setShopId(values.shopId);
    }, [setShopId]);

    const onAddProductFormSubmit = useCallback((values: AddProductForm) => {
        const product: CreateProductRequest = {
            name: values.name,
            url: values.url,
            price: {
                currencyId: values.currencyId,
                price: values.price,
            },
            description: values.description,
        };

        if (values.sale) {
            product.price.discountedPrice = values.discountedPrice;
        }

        setProducts(products => ([
            ...products,
            product,
        ]));
    }, [setProducts]);

    const [createComposition] = useCreateCompositionMutation();

    const canFinish = shopId && image && products.length > 0;
    const onFinishClick = () => {
        if (canFinish) {
            const request: CreateCompositionRequest = {
                shopId: shopId,
                image: image,
                products: products,
            };
            createComposition(request);
        }
    };

    return (
        <>
            <Row className="mb-3">
                <Col className="d-flex justify-content-end">
                    <Button disabled={!canFinish} onClick={onFinishClick}>Завершити</Button>
                </Col>
            </Row>
            <Row className="mb-3">
                <Col xs={6}>
                    <Form<ImageFormValues>
                        onSubmit={onImageFormSubmit}
                        render={({ handleSubmit }) => (
                            <BootstrapForm onSubmit={handleSubmit}>
                                <PickImageStep />
                            </BootstrapForm>
                        )}
                    />
                </Col>
                <Col xs={6} className="d-flex flex-column justify-content-center">
                    <Form<ShopFormValues>
                        onSubmit={onShopFormSubmit}
                        render={({ handleSubmit }) => (
                            <BootstrapForm onSubmit={handleSubmit}>
                                <SelectShopStep />
                            </BootstrapForm>
                        )}
                    />
                </Col>
            </Row>
            <Row>
                <Col xs={6}>
                    <Row>
                        {products.map((product, index) => (
                            <Col key={index} xs={6} className="mb-3">
                                <Product product={product} />
                            </Col>
                        ))}
                    </Row>
                </Col>
                <Col xs={6}>
                    <Form<AddProductForm>
                        onSubmit={onAddProductFormSubmit}
                        render={({ handleSubmit }) => (
                            <BootstrapForm onSubmit={handleSubmit}>
                                <AddProduct />
                            </BootstrapForm>
                        )}
                    />
                </Col>
            </Row>
        </>
    );
};

export default AddAnnounce;