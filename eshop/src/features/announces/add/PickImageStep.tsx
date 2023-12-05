import React, { useEffect } from "react";
import { Field, useField, useForm } from "react-final-form";
import { Form as BootstrapForm, Card } from "react-bootstrap";
import FileField from "../../../components/FileField";

export interface ImageFormValues {
    image: File,
}

const PickImageStep: React.FC = () => {
    const {
        input: {
            value: image,
        },
    } = useField("image", {
        subscription: {
            value: true,
        },
    });

    const form = useForm();

    useEffect(() => {
        if (image) {
            form.submit();
        }
    }, [image, form]);

    return (

        <BootstrapForm.Group controlId="image">
            <BootstrapForm.Label className="d-block">
                <Card>
                    <Card.Body className="d-flex align-items-center justify-content-center position-relative" style={{ height: 440 }}>
                        <div className={image ? "d-none" : ""}>
                            <span className="d-block fs-6 mb-1">Завантажте зображення для анонсу</span>
                            <Field
                                name="image"
                                accept=".png,.jpg,.jpeg"
                                component={FileField}
                            />
                        </div>
                        {image && (
                            <img className="position-absolute w-100 h-100" src={URL.createObjectURL(image)} style={{ objectFit: "contain"}} alt=""/>
                        )}
                    </Card.Body>
                </Card>
            </BootstrapForm.Label>
        </BootstrapForm.Group>
    );
};

export default PickImageStep;