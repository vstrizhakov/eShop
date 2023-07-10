import { Form } from "react-bootstrap";
import { FieldRenderProps } from "react-final-form";

interface IProps extends FieldRenderProps<string> {
    id?: string,
};

const Select = (props: IProps) => {
    return (
        <Form.Select
            {...props}
            {...props.input}/>
    );
};

export default Select;