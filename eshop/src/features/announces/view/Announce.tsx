import React, { useEffect, useMemo } from "react";
import { Announce as AnnounceModel } from "../../api/catalogSlice";
import { HttpTransportType, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import DistributionContainer from "./DistributionContainer";
import { RootState } from "../../../app/store";
import { ConnectedProps, connect } from "react-redux";
import { setAnnounce } from "./viewAnnounceSlice";
import { Badge, Col, ListGroup, ListGroupItem, Row } from "react-bootstrap";

const mapStateToProps = (state: RootState) => ({
    accessToken: state.auth.token,
});

const mapDispatchToProps = {
    setAnnounce,
};

const connector = connect(mapStateToProps, mapDispatchToProps);

type PropsFromRedux = ConnectedProps<typeof connector>;
interface IProps extends PropsFromRedux {
    announce: AnnounceModel,
};

const Announce: React.FC<IProps> = props => {
    const {
        announce,
        setAnnounce,
        accessToken,
    } = props;

    const mainImage = announce.images[0];

    return (
        <>
            <Row className="mb-5">
                <Col>
                    <h4 className="mb-3">Анонс #1</h4>
                    <img className="rounded" src={mainImage} width="100%" />
                </Col>
                <Col>
                    <h4 className="d-flex justify-content-between align-items-center mb-3">
                        <span className="text-muted">{announce.shop.name} - Позиції</span>
                        <Badge bg="secondary" pill>{announce.products.length}</Badge>
                    </h4>
                    <ListGroup>
                        {announce.products.map(product => (
                            <ListGroupItem key={product.id} className="d-flex justify-content-between align-items-center">
                                <div>
                                    <strong>{product.name}</strong>
                                    {product.description && (
                                        <p className="text-muted">
                                            <small>
                                                {product.description}
                                            </small>
                                        </p>
                                    )}
                                </div>
                                <div className="d-flex gap-2">
                                    {!product.price.discountedPrice && (
                                        <span>{product.price.price} {product.price.currency.name}</span>
                                    )}
                                    {product.price.discountedPrice && (
                                        <>
                                            <s>{product.price.price} {product.price.currency.name}</s>
                                            <span>{product.price.discountedPrice} {product.price.currency.name}</span>
                                        </>
                                    )}
                                </div>
                            </ListGroupItem>
                        ))}
                    </ListGroup>
                </Col>
            </Row>

            {announce.distributionId && (
                <DistributionContainer distributionId={announce.distributionId} />
            )}
        </>
    );
};

export default connector(Announce);