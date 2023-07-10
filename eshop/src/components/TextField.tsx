import { Form } from "react-bootstrap";
import { FieldRenderProps } from "react-final-form";

const TextField = (props: FieldRenderProps<string>) => {
    return (
        <Form.Control
            {...props.input}
            {...props} />
    );
};

export default TextField;