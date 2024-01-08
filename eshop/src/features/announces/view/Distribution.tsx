import React, { useEffect } from "react";
import { Card, Col, ProgressBar, Row } from "react-bootstrap";
import { DeliveryStatus, DistributionItem, Distribution as DistributionModel } from "../../api/distributionSlice";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { RootState } from "../../../app/store";
import { ConnectedProps, connect } from "react-redux";
import { updateDistributionItem, reset } from "./viewAnnounceSlice";

const mapStateToProps = (state: RootState) => ({
    accessToken: state.auth.token,
});

const mapDispatchToProps = {
    updateDistributionItem,
    reset,
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

    useEffect(() => {
        return () => {
            reset();
        };
    }, []);

    useEffect(() => {
        let baseUrl = "";
        if (process.env.REACT_APP_ORIGIN) {
            baseUrl = `https://${process.env.REACT_APP_ORIGIN}`;
        }

        const connection = new HubConnectionBuilder()
            .withUrl(baseUrl + "/api/distribution/ws", {
                accessTokenFactory: () => accessToken!,
            })
            .withAutomaticReconnect()
            .build();

        let canceled = false;
        const task = (async function () {
            connection.on("requestUpdated", (item: DistributionItem) => {
                updateDistributionItem(item);
            });

            await connection.start();

            if (!canceled) {
                await connection.invoke("subscribe", {
                    distributionId: distribution.id,
                });
            }
        })();

        return () => {
            (async function () {
                canceled = true;

                await task;

                await connection.stop()
            })();
        };
    }, [accessToken, distribution.id, updateDistributionItem]);

    const items = distribution.recipients.reduce<DistributionItem[]>((prev, current) => [...prev, ...current.items], []);
    const totalInProgress = items.filter(item => item.deliveryStatus === DeliveryStatus.Pending).length > 0;
    const totalAnyFailed = items.filter(item => item.deliveryStatus === DeliveryStatus.Failed).length > 0;

    const totalStatus = totalAnyFailed ? DeliveryStatus.Failed : totalInProgress ? DeliveryStatus.Pending : DeliveryStatus.Delivered;

    const getVariant = (deliveryStatus: DeliveryStatus): string => {
        switch (deliveryStatus) {
            case DeliveryStatus.Pending:
                return "info";
            case DeliveryStatus.Delivered:
                return "success";
            case DeliveryStatus.Failed:
                return "danger";
            case DeliveryStatus.Filtered:
                return "secondary";
        }
    };

    const recipients = distribution.recipients.filter(recipient => recipient.items.filter(item => item.deliveryStatus !== DeliveryStatus.Filtered).length > 0);
    const totalRecipients = recipients.length;
    const finishedRecipients = recipients.filter(recipient => recipient.items.every(item => item.deliveryStatus !== DeliveryStatus.Pending)).length;

    return (
        <>
            <div className="d-flex align-items-center flex-row gap-3 mb-3">
                <h4>
                    <span>Отримувачі</span>
                </h4>
                <ProgressBar className="flex-grow-1" variant={getVariant(totalStatus)} now={100} animated label={`${finishedRecipients}/${totalRecipients}`}></ProgressBar>
            </div>
            <Row>
                {recipients.map(recipient => {
                    const items = recipient.items.filter(item => item.deliveryStatus !== DeliveryStatus.Filtered);
                    const totalItems = items.length;
                    const finishedItems = items.filter(item => item.deliveryStatus !== DeliveryStatus.Pending).length;

                    const inProgress = items.filter(item => item.deliveryStatus === DeliveryStatus.Pending).length > 0;
                    const anyFailed = items.filter(item => item.deliveryStatus === DeliveryStatus.Failed).length > 0;

                    const status = anyFailed ? DeliveryStatus.Failed : inProgress ? DeliveryStatus.Pending : DeliveryStatus.Delivered;

                    return (
                        <Col md={6} lg={4} xl={3} className="mb-1" key={recipient.client.id}>
                            <Card>
                                <Card.Body>
                                    <div className="d-flex align-items-center flex-row gap-3">
                                        <strong>{recipient.client.firstName} {recipient.client.lastName}</strong>
                                        <ProgressBar className="flex-grow-1" variant={getVariant(status)} now={100} animated label={`${finishedItems}/${totalItems}`} />
                                    </div>
                                </Card.Body>
                            </Card>
                        </Col>
                    );
                })}
            </Row>
        </>
    );
};

export default connector(Distribution);