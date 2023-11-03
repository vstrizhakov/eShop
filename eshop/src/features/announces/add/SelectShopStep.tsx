import React, { useEffect } from "react";
import { Form as BootstrapForm } from "react-bootstrap";
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
        <BootstrapForm.Group controlId="shop-select">
            <center>
                <div>
                    <BootstrapForm.Label className="fw-semibold fs-2 mb-3">Оберіть магазин анонсу</BootstrapForm.Label>
                    <Field
                        name="shopId"
                        disabled={shops === undefined}
                        defaultValue={shops && shops.length > 0 && shops[0].id}
                        style={{ width: 320 }}
                        component={Select}>
                        {shops && shops.map(shop => (
                            <option key={shop.id} value={shop.id}>{shop.name}</option>
                        ))}
                    </Field>
                </div>
            </center>
        </BootstrapForm.Group>
    );
};

export default SelectShopStep;