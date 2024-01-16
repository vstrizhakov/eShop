import React, { useEffect } from "react";
import { Form as BootstrapForm, Col } from "react-bootstrap";
import { Field, useField, useForm } from "react-final-form";
import Select from "../../../components/Select";
import { useGetShopsQuery } from "../../api/catalogSlice";

export interface ShopFormValues {
    shopId: string,
}

const SelectShopStep: React.FC = () => {
    const {
        input: {
            value: shop,
        },
    } = useField("shopId", {
        subscription: {
            value: true,
        },
    });

    const form = useForm();

    const {
        data: shops,
    } = useGetShopsQuery(undefined);

    useEffect(() => {
        if (shop) {
            form.submit();
        }
    }, [shop, form]);

    return (
        <BootstrapForm.Group controlId="shop-select" className="row align-items-center">
            <BootstrapForm.Label column={true} md="auto">Оберіть магазин анонсу</BootstrapForm.Label>
            <Col sm={12} md={6} lg={5} xl={4} xxl={3}>
                <Field
                    name="shopId"
                    list="shops"
                    disabled={shops === undefined}
                    defaultValue={shops && shops.length > 0 && shops[0].id}
                    required={true}
                    component={Select}>
                    {shops && shops.map(shop => (
                        <option key={shop.id} value={shop.id}>{shop.name}</option>
                    ))}
                </Field>
            </Col>
        </BootstrapForm.Group>
    );
};

export default SelectShopStep;