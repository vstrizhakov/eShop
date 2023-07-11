import React from "react";
import { useGetClientsQuery } from "../api/distributionSlice";
import { Col, Row } from "react-bootstrap";

const Clients: React.FC = () => {
    const {
        data: clients,
    } = useGetClientsQuery(undefined);

    return (
        <>
            {(clients && clients.map(client => (
                <div>
                    Client #{client.id}
                    <Row>
                        <Col>
                            <strong>Telegram Chats</strong>
                            {client.telegramChats.map(telegramChat => (
                                <div>#{telegramChat.id}, isEnabled = {telegramChat.isEnabled.toString()}</div>
                            ))}
                        </Col>
                        {client.viberChat && (
                            <Col>
                                <strong>Viber Chat</strong>
                                <div>#{client.viberChat.id}, isEnabled = {client.viberChat.isEnabled.toString()}</div>
                            </Col>
                        )}
                    </Row>
                </div>
            )))}
        </>
    );
};

export default Clients;