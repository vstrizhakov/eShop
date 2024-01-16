import React, { useCallback, useState } from "react";
import { Col, Row, Form as BootstrapForm, Spinner, Button } from "react-bootstrap";
import PickImageStep, { ImageFormValues } from "./PickImageStep";
import SelectShopStep, { ShopFormValues } from "./SelectShopStep";
import AddProduct, { AddProductForm } from "./AddProduct";
import { Form } from "react-final-form";
import { FormApi } from "final-form";
import { CreateAnnounceRequest, CreateProductRequest, useCreateAnnounceMutation, useGetCurrenciesQuery } from "../../api/catalogSlice";
import Product from "../Product";
import { useNavigate } from "react-router-dom";
import LoadingButton from "../../../components/LoadingButton";

const AddAnnounce: React.FC = () => {
    const [image, setImage] = useState<File | undefined>(undefined);
    const [shopId, setShopId] = useState<string | undefined>(undefined);
    const [products, setProducts] = useState<CreateProductRequest[]>([]);

    const {
        data: currencies,
    } = useGetCurrenciesQuery(undefined);

    const onImageFormSubmit = useCallback((values: ImageFormValues) => {
        setImage(values.image);
    }, [setImage]);

    const onShopFormSubmit = useCallback((values: ShopFormValues) => {
        setShopId(values.shopId);
    }, [setShopId]);

    const onAddProductFormSubmit = useCallback((values: AddProductForm, form: FormApi<AddProductForm, Partial<AddProductForm>>) => {
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

        form.restart({
            currencyId: values.currencyId,
            sale: values.sale,
        });
    }, [setProducts]);

    const [createAnnounce, {
        isLoading: isCreatingAnnounce,
    }] = useCreateAnnounceMutation();

    const navigate = useNavigate();

    const canFinish = shopId && image && products.length > 0;
    const onFinishClick = async () => {
        if (canFinish) {
            const request: CreateAnnounceRequest = {
                shopId: shopId,
                image: image,
                products: products,
            };

            const announce = await createAnnounce(request).unwrap();
            navigate(`/announces/${announce.id}`);
        }
    };

    const onDeleteProductClick = (product: CreateProductRequest) => () => {
        const newProducts = products.reduce<CreateProductRequest[]>((prev, current) => {
            if (current !== product) {
                prev.push(current);
            }
            return prev;
        }, []);
        setProducts(newProducts);
    };

    return (
        <>
            <Row className="flex-wrap-reverse">
                <Col className="mb-3">
                    <Form<ShopFormValues>
                        onSubmit={onShopFormSubmit}
                        render={({ handleSubmit }) => (
                            <BootstrapForm onSubmit={handleSubmit}>
                                <SelectShopStep />
                            </BootstrapForm>
                        )}
                    />
                </Col>
                <Col md={3} lg={2} className="mb-3">
                    <LoadingButton
                        disabled={!canFinish}
                        isLoading={isCreatingAnnounce}
                        onClick={onFinishClick}
                        className="fw-semibold w-100">
                        Опублікувати
                    </LoadingButton>
                </Col>
            </Row>
            <Row className="mb-3">
                <Col lg={6}>
                    <Form<ImageFormValues>
                        onSubmit={onImageFormSubmit}
                        render={({ handleSubmit }) => (
                            <BootstrapForm onSubmit={handleSubmit}>
                                <PickImageStep />
                            </BootstrapForm>
                        )}
                    />
                </Col>
                <Col lg={6}>
                    {currencies ? (
                        <Form<AddProductForm>
                            initialValues={{
                                currencyId: currencies[0]?.id,
                            }}
                            onSubmit={onAddProductFormSubmit}
                            render={({ handleSubmit }) => (
                                <form noValidate={true} onSubmit={handleSubmit}>
                                    <AddProduct currencies={currencies} />
                                </form>
                            )}
                        />
                    ) : (
                        <Spinner />
                    )}
                </Col>
            </Row>

            <h4 className="mb-3">Позиції</h4>
            {products.length > 0 ? (
                <Row>
                    {products.map((product, index) => (
                        <Col key={index} md={6} lg={4} xxl={3} className="mb-3">
                            <div className="position-relative">
                                <Product
                                    name={product.name}
                                    url={product.url}
                                    price={product.price.price}
                                    discountedPrice={product.price.discountedPrice}
                                    currencyName={currencies?.find(currency => currency.id === product.price.currencyId)?.name!}
                                    description={product.description}
                                />
                                <Button variant="link" className="text-danger position-absolute top-0 end-0 mt-2" onClick={onDeleteProductClick(product)}>
                                    <i className="bi bi-trash"></i>
                                </Button>
                            </div>
                        </Col>
                    ))}
                </Row>
            ) : (
                <p>Ви не додали жодних позицій</p>
            )}
        </>
    );
};

export default AddAnnounce;