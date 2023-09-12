import React from "react";
import { useActivateClientMutation, useDeactivateClientMutation, useGetClientsQuery } from "../api/distributionSlice";
import { Button, Col, Collapse, Form, InputGroup, ListGroup, ListGroupItem, Row, Stack } from "react-bootstrap";
import useAuth from "../auth/useAuth";
import MyInvitation from "./MyInvitation";

const Clients: React.FC = () => {
    const {
        data: clients,
    } = useGetClientsQuery(undefined);

    const [activateClient] = useActivateClientMutation();
    const [deactivateClient] = useDeactivateClientMutation();

    return (
        <>
            <MyInvitation />
            {clients && (
                <ListGroup className="mt-5">
                    {clients.map(client => (
                        <ListGroupItem key={client.id} className="py-2">
                            <Row>
                                <Col xs={6}>
                                    <strong>{client.firstName} {client.lastName}</strong>
                                </Col>
                                <Col xs={3}>
                                </Col>
                                <Col xs={3}>
                                    <Stack direction="horizontal" gap={3}>
                                        <Button variant="outline-secondary" disabled={client.isActivated} onClick={() => activateClient(client.id)}>Активувати</Button>
                                        <Button variant="outline-secondary" disabled={!client.isActivated} onClick={() => deactivateClient(client.id)}>Деактивувати</Button>
                                    </Stack>
                                </Col>
                            </Row>
                        </ListGroupItem>
                    ))}
                </ListGroup>
            )}
        </>
    );
};

export default Clients;