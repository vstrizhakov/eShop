import React, { useEffect } from "react";
import { Field, useField, useForm } from "react-final-form";
import { Form as BootstrapForm, Card } from "react-bootstrap";
import FileField from "../../components/FileField";

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
        <Card>
            <Card.Body className="d-flex align-items-center justify-content-center" style={{ height: 440 }}>
                <BootstrapForm.Group controlId="image" className="mb-3">
                    <center>
                        <BootstrapForm.Label className="fw-semibold fs-2 mb-3">Додайте фотографію анонсу</BootstrapForm.Label>
                        <Field
                            name="image"

                            accept=".png,.jpg,.jpeg"
                            component={FileField}
                        />
                    </center>
                </BootstrapForm.Group>
            </Card.Body>
        </Card>
    );
};

export default PickImageStep;