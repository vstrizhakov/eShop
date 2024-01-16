import React from "react";
import { useGetAnnouncesQuery } from "../api/catalogSlice";
import { Anchor, Col, Row, Spinner } from "react-bootstrap";
import AnnounceListItem from "./AnnounceListItem";
import { LinkContainer } from "react-router-bootstrap";

const AnnounceList: React.FC = () => {
    const {
        data: announces,
        isError,
    } = useGetAnnouncesQuery(undefined);


    if (isError) {
        return <>Під час завантаження сталася помилка</>;
    }

    if (!announces) {
        return (
            <div className="d-flex w-100 justify-content-center">
                <Spinner />
            </div>
        );
    }

    return (
        <Row>
            {announces.map(announce => (
                <Col key={announce.id} lg={6} xxl={4} className="mb-2">
                    <LinkContainer to={`/announces/${announce.id}`}>
                        <Anchor className="text-decoration-none">
                            <AnnounceListItem announce={announce} />
                        </Anchor>
                    </LinkContainer>
                </Col>
            ))}
        </Row>
    );
};

export default AnnounceList;