import React, { useCallback, useState } from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Anchor, Button, Col, Container, Navbar, Row } from "react-bootstrap";
import Invitation from "./Invitation";
import Compositions from "./Compositions";
import Clients from "./Clients";
import { Composition } from "../api/catalogSlice";
import CompositionComponent from "./Composition";
import { Link, RouterProvider, createBrowserRouter } from "react-router-dom";
import AddAnnonce from "./AddAnonce";
import { LinkContainer } from "react-router-bootstrap";

const Main: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
        claims,
        signIn,
    } = props;

    const [selectedComposition, setSelectedComposition] = useState<Composition | undefined>();

    const onCompositionSelected = useCallback((composition: Composition) => {
        setSelectedComposition(composition);
    }, [setSelectedComposition]);

    return (
        <>
            {isAuthenticated && (
                <>
                    <div className="d-flex align-items-center justify-content-center" style={{ height: 240 }}>
                        <LinkContainer to="/addAnnonce">
                            <Button size="lg" className="fw-semibold" variant="outline-primary border-start-0 border-end-0 rounded-0 text-white">
                                ДОДАТИ АНОНС
                            </Button>
                        </LinkContainer>
                    </div>
                    {/* <Clients />
                        <Invitation />
                        <CreateCurrency />
                        <Row>
                            <Col>
                                <Compositions onCompositionSelected={onCompositionSelected} />
                                {selectedComposition && (
                                    <CompositionComponent composition={selectedComposition} />
                                )}
                            </Col>
                            <Col>
                                <CreateComposition />
                            </Col>
                        </Row> */}
                </>
            )}
        </>
    );
};

export default withAuth(Main);