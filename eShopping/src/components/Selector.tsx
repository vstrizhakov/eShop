import { useCallback } from "react";
import { Card, Col, Row } from "react-bootstrap";
import { FieldRenderProps } from "react-final-form";

interface IProps<IItem> extends FieldRenderProps<IItem> {
    id?: string,
    className?: string,
    items: IItem[],
    renderItem: (item: IItem) => JSX.Element,
};

const Selector = <IItem,>(props: IProps<IItem>) => {
    const {
        id,
        className,
        items,
        renderItem,
        input,
    } = props;

    const {
        value,
        onChange,
    } = input;

    const onItemSelected = useCallback((item: IItem) => () => {
        onChange(item);
    }, [onChange]);

    return (
        <Card id={id} className={className}>
            <Card.Body className="py-2">
                <Row>
                    {items.map((item: any, index) => {
                        let border, bg;
                        if (item === value) {
                            border = "primary";
                            bg = "primary-subtle";
                        }

                        return (
                            <Col key={index} xs={3}>
                                <Card border={border} bg={bg} className="my-2 selector-item" onClick={onItemSelected(item)}>
                                    <Card.Body>
                                        {renderItem(item)}
                                    </Card.Body>
                                </Card>
                            </Col>
                        );
                    })}
                </Row>
            </Card.Body>
        </Card>
    );
};

export default Selector;
