import React from "react";
import { useGetAnnouncesQuery } from "../api/catalogSlice";
import { Spinner } from "react-bootstrap";
import AnnounceListItem from "./AnnounceListItem";

const AnnounceList: React.FC = () => {
    const {
        data: announces,
        isError,
    } = useGetAnnouncesQuery(undefined);


    if (isError) {
        return <>Під час завантаження сталася помилка</>;
    }

    if (!announces) {
        return <Spinner />;
    }

    return (
        <>
            {announces.map(announce => (
                <AnnounceListItem announce={announce} />
            ))}
        </>
    );
};

export default AnnounceList;