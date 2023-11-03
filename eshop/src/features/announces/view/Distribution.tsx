import React, { Fragment, useEffect, useMemo } from "react";
import { Badge, Card, Col, ProgressBar, Row, Table } from "react-bootstrap";
import { DeliveryStatus, DistributionItem, Distribution as DistributionModel } from "../../api/distributionSlice";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { RootState } from "../../../app/store";
import { ConnectedProps, connect } from "react-redux";
import { updateDistributionItem } from "./viewAnnounceSlice";

const mapStateToProps = (state: RootState) => ({
    accessToken: state.auth.token,
});

const mapDispatchToProps = {
    updateDistributionItem,
};

const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;

interface IProps extends PropsFromRedux {
    distribution: DistributionModel,
};

const Distribution: React.FC<IProps> = props => {
    const {
        accessToken,
        distribution,
        updateDistributionItem,
    } = props;

    const connection = useMemo(() => {
        const connection = new HubConnectionBuilder()
            .withUrl("/api/distribution/ws", {
                accessTokenFactory: () => accessToken!,
            })
            .withAutomaticReconnect()
            .build();
        return connection;
    }, [])

    useEffect(() => {
        const initialize = async () => {
            if (connection.state === HubConnectionState.Disconnected) {
                connection.on("requestUpdated", (item: DistributionItem) => {
                    updateDistributionItem(item);
                });

                await connection.start();

                //if (connection.state === HubConnectionState.Connected) {
                await connection.invoke("subscribe", {
                    distributionId: distribution.id,
                });
                //}
            }
        };

        const uninitialize = async () => {
            if (connection.state === HubConnectionState.Connected) {
                await connection.invoke("unsubscribe", {
                    distributionId: distribution.id,
                });

                await connection.stop();
            }
        }

        initialize();

        return () => {
            uninitialize();
        };
    }, [connection]);

    const items = distribution.recipients.reduce<DistributionItem[]>((prev, current) => [...prev, ...current.items], []);
    const inProgress = items.filter(item => item.deliveryStatus === DeliveryStatus.Pending).length > 0;
    const anyFailed = items.filter(item => item.deliveryStatus === DeliveryStatus.Failed).length > 0;

    const totalStatus = inProgress ? DeliveryStatus.Pending : anyFailed ? DeliveryStatus.Failed : DeliveryStatus.Delivered;

    const getVariant = (deliveryStatus: DeliveryStatus): string => {
        switch (deliveryStatus) {
            case DeliveryStatus.Pending:
                return "info";
            case DeliveryStatus.Delivered:
                return "success";
            case DeliveryStatus.Failed:
                return "danger";
        }
    }
    return (
        <>
            <div className="d-flex align-items-center flex-row gap-3 mb-3">
                <h4>
                    <span>Отримувачі</span>
                    <Badge className="ms-2" bg="secondary" pill>{distribution.recipients.length}</Badge>
                </h4>
                <ProgressBar className="flex-grow-1" variant={getVariant(totalStatus)} now={100} animated />
            </div>
            <Row>
                {distribution.recipients.map(recipient => (
                    <Col xs={4} key={recipient.client.id}>
                        <Card>
                            <Card.Header>
                                {recipient.client.firstName} {recipient.client.lastName}
                            </Card.Header>
                            <Card.Body>
                                {recipient.items.map(item => (
                                    <Fragment key={item.id}>
                                        {item.telegramChatId && (
                                            <div className="d-flex align-items-center flex-row gap-3">
                                                <span>Telegram</span>
                                                <ProgressBar className="w-100" variant={getVariant(item.deliveryStatus)} animated now={100} />
                                            </div>
                                        )}
                                        {item.viberChatId && (
                                            <div className="d-flex align-items-center flex-row gap-3">
                                                <span>Viber</span>
                                                <ProgressBar className="w-100" variant={getVariant(item.deliveryStatus)} animated now={100} />
                                            </div>
                                        )}
                                    </Fragment>
                                ))}
                            </Card.Body>
                        </Card>
                    </Col>
                ))}
            </Row>
        </>
    );
};

export default connector(Distribution);