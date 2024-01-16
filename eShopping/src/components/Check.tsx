import { Form } from "react-bootstrap";
import { FormCheckType } from "react-bootstrap/esm/FormCheck";
import { FieldRenderProps } from "react-final-form";

interface IProps<T> extends FieldRenderProps<T> {
    type: FormCheckType,
};

const Check = <T extends string>(props: IProps<T>) => {
    return (
        <Form.Check
            {...props.input}
            {...props} />
    );
};

export default Check;