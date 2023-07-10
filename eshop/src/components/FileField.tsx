import { Form } from "react-bootstrap";
import { FieldRenderProps } from "react-final-form";

const FileField = (props: FieldRenderProps<string>) => {
    return (
        <Form.Control
            {...props.input}
            {...props}
            value={undefined}
            type="file"
            onChange={({ target }) => {
                const input = target as HTMLInputElement;
                props.input.onChange(input.files?.item(0));
            }}
        />
    );
};

export default FileField;