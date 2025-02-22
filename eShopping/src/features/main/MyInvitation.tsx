import React from "react";
import { InputGroup, Button, Form } from "react-bootstrap";
import useAuth from "../auth/useAuth";

const MyInvitation: React.FC = () => {
    const {
        claims,
    } = useAuth();

    const providerId = claims!.account_id;
    const announcerLink = `${window.location.origin}/announcer/${providerId}`;

    const onCopyClicked = () => {
        navigator.clipboard.writeText(announcerLink);
    };

    return (
        <Form.Group controlId="anouncer-link">
            <Form.Label>Посилання на Ваше запрошення:</Form.Label>
            <InputGroup>
                <Form.Control readOnly value={announcerLink} />
                <Button variant="outline-secondary" onClick={onCopyClicked}>
                    <i className="bi bi-file-earmark" />
                </Button>
            </InputGroup>
        </Form.Group>
    );
};

export default MyInvitation;