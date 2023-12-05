import React from "react";
import { Announce } from "../api/catalogSlice";
import { Badge, Card, Col, Row } from "react-bootstrap";

interface IProps {
    announce: Announce,
};

const AnnounceListItem: React.FC<IProps> = props => {
    const {
        announce: {
            images,
            shop: {
                name: shopName,
            },
            products,
            createdAt,
        },
    } = props;

    const mainImage = images[0];
    const createdAtDatetime = new Date(createdAt);
    const announceName = `Анонс від ${createdAtDatetime.getDate()}.${createdAtDatetime.getMonth() + 1}.${createdAtDatetime.getFullYear()}`;

    return (
        <Card className="bg-body-tertiary border-0 rounded-5 shadow">
            <Card.Body>
                <Row>
                    <Col xs={5}>
                        <div style={{ height: 120 }} className="d-flex align-items-center justify-content-center">
                            <img src={mainImage} style={{ maxWidth: "100%", maxHeight: "100%" }} className="rounded" alt={announceName}/>
                        </div>
                    </Col>
                    <Col xs={7}>
                        <div className="d-flex flex-column h-100 w-100">
                            <strong className="fw-semibold text-body-emphasis">{announceName}</strong>
                            <small>Позицій: {products.length}</small>
                            <div className="mt-auto d-flex justify-content-between align-items-center">
                                <Badge bg="info" pill className="fw-semibold">{shopName}</Badge>
                                <small className="text-muted">{createdAtDatetime.toLocaleTimeString()}</small>
                            </div>
                        </div>
                    </Col>
                </Row>
            </Card.Body>
        </Card>
    );
};

export default AnnounceListItem;