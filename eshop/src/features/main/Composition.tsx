import React, { useEffect } from "react";
import { useGetCompositionQuery } from "../api/catalogSlice";
import Distribution from "./Distribution";
import { useParams } from "react-router-dom";
import { HttpTransportType, HubConnectionBuilder } from "@microsoft/signalr";


const Composition: React.FC = props => {
    const {
        announceId,
    } = useParams();

    const {
        data: composition,
        isLoading,
        isSuccess,
    } = useGetCompositionQuery(announceId!);

    useEffect(() => {
        const invoke = async () => {
            if (composition) {
                const connection = new HubConnectionBuilder()
                    .withUrl("/api/distribution/notifications", HttpTransportType.WebSockets)
                    .withAutomaticReconnect()
                    .build();

                connection.on("requestUpdated", (request: any) => {
                    console.log("requestUpdated", request);
                });

                await connection.start();

                connection.invoke("subscribe", composition.distributionGroupId);
            }
        };

        invoke();
    }, [composition]);

    if (isLoading) {
        return <>"Завантаження..."</>;
    }

    if (!isSuccess) {
        return <>"Під час завантаження анонсу сталася помилка"</>;
    }

    return (
        <div>
            Анонс #{composition.id}
        </div>
    );
};

export default Composition;