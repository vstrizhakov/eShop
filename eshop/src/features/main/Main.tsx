import React, { useCallback, useState } from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Anchor, Button, Col, Container, Navbar, Row } from "react-bootstrap";
import Invitation from "./Invitation";
import Compositions from "./Compositions";
import CreateComposition from "./CreateComposition";
import CreateCurrency from "./CreateCurrency";
import Clients from "./Clients";
import { Composition } from "../api/catalogSlice";
import CompositionComponent from "./Composition";
import "./Main.scss";

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
            <Navbar>
                <Container >
                    <Navbar.Brand>eShop</Navbar.Brand>
                    <Navbar.Toggle />
                    <Navbar.Collapse className="justify-content-end">
                        {isAuthenticated ? (
                            <Navbar.Text>Volodymyr Stryzhakov</Navbar.Text>
                        ) : (
                            <Button onClick={signIn} size="sm">
                                Sign In
                            </Button>
                        )}
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container>
                {isAuthenticated && (
                    <>
                        <div className="d-flex align-items-center justify-content-center" style={{ height: 320 }}>
                            <Button href="#" size="lg" className="text-white fw-semibold">
                                ДОДАТИ АНОНС
                            </Button>
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
            </Container>
        </>
    );
};

export default withAuth(Main);