import React from "react";
import { useGetClientsQuery } from "../api/distributionSlice";
import { Button, Col, Collapse, Form, InputGroup, ListGroup, ListGroupItem, Row } from "react-bootstrap";
import useAuth from "../auth/useAuth";
import MyInvitation from "./MyInvitation";

const Clients: React.FC = () => {
    const {
        data: clients,
    } = useGetClientsQuery(undefined);


    return (
        <>
            <MyInvitation />
            {clients && (
                <ListGroup className="mt-5">
                    {clients.map(client => (
                        <ListGroupItem key={client.id} className="py-3">
                            <strong>{client.firstName} {client.lastName}</strong>
                        </ListGroupItem>
                    ))}
                </ListGroup>
            )}
        </>
    );
};

export default Clients;