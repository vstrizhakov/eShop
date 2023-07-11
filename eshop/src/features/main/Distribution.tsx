import React from "react";
import { useGetDistributionQuery } from "../api/distributionSlice";
import { Table } from "react-bootstrap";

interface IProps {
    distributionGroupId: string,
};

const Distribution: React.FC<IProps> = props => {
    const {
        distributionGroupId,
    } = props;

    const {
        data: distribution,
    } = useGetDistributionQuery(distributionGroupId);

    return (
        <Table>
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Telegram Chat Id</th>
                    <th>Viber Chat Id</th>
                    <th>Delivery Status</th>
                </tr>
            </thead>
            <tbody>
                {distribution && distribution.items.map(distributionItem => (
                    <tr>
                        <td>{distributionItem.id}</td>
                        <td>{distributionItem.telegramChatId}</td>
                        <td>{distributionItem.viberChatId}</td>
                        <td>{distributionItem.deliveryStatus}</td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default Distribution;