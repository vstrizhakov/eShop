import React from "react";
import { useActivateClientMutation, useDeactivateClientMutation, useGetClientsQuery } from "../api/distributionSlice";
import { Badge, Button, Card, Col, ListGroup, ListGroupItem, Row, Spinner, Stack } from "react-bootstrap";
import MyInvitation from "./MyInvitation";
import { ReactComponent as Telegram } from "../../assets/telegram.svg";
import { ReactComponent as Viber } from "../../assets/viber.svg";

const Clients: React.FC = () => {
    const {
        data: clients,
        isLoading: clientsLoading,
    } = useGetClientsQuery(undefined);

    const [activateClient] = useActivateClientMutation();
    const [deactivateClient] = useDeactivateClientMutation();

    return (
        <Stack className="gap-5">
            <MyInvitation />
            <Stack>
                <h4>Мої клієнти</h4>
                {clientsLoading && (
                    <Spinner className="align-self-center my-2" />
                )}
                <Row className="row-gap-3">
                    {clients && clients.map(client => (
                        <Col xxl={6}>
                            <Card key={client.id}>
                                <Card.Body>
                                    <Row>
                                        <Col xs={7} sm={8} md>
                                            <Row>
                                                <Col className="d-flex align-items-center">
                                                    <strong>{client.firstName} {client.lastName}</strong>
                                                </Col>
                                                <Col md="auto" className="d-flex align-items-center">
                                                    <Row>
                                                        <Col>
                                                            <Stack direction="horizontal" gap={1}>
                                                                {client.viberChat && (
                                                                    <Viber fill="white" height="36px" />
                                                                )}
                                                                {client.telegramChats.length > 0 && (
                                                                    <div className="position-relative">
                                                                        <Telegram fill="white" height="36px" />
                                                                        <Badge
                                                                            className="position-absolute"
                                                                            bg="info"
                                                                            style={{
                                                                                bottom: -4,
                                                                                right: -6,
                                                                                fontSize: 10,
                                                                                paddingTop: 2,
                                                                                paddingBottom: 2,
                                                                            }}>
                                                                            {client.telegramChats.length}
                                                                        </Badge>
                                                                    </div>
                                                                )}
                                                            </Stack>
                                                        </Col>
                                                    </Row>
                                                </Col>
                                            </Row>
                                        </Col>
                                        <Col xs={5} sm={4} md="auto">
                                            <Row className="row-gap-1">
                                                <Col md="auto">
                                                    <Button className="w-100" variant="outline-secondary" disabled={client.isActivated} onClick={() => activateClient(client.id)}>Активувати</Button>
                                                </Col>
                                                <Col md="auto">
                                                    <Button className="w-100" variant="outline-secondary" disabled={!client.isActivated} onClick={() => deactivateClient(client.id)}>Деактивувати</Button>
                                                </Col>
                                            </Row>
                                        </Col>
                                    </Row>
                                </Card.Body>
                            </Card>
                        </Col>
                    ))}
                </Row>
            </Stack>
        </Stack>
    );
};

export default Clients;