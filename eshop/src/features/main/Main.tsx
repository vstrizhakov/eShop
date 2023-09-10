import React, { Suspense, useCallback, useState } from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Button } from "react-bootstrap";
import { Composition } from "../api/catalogSlice";
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
                        <LinkContainer to="/addAnnounce">
                            <Button size="lg" className="fw-semibold" variant="outline-primary border-start-0 border-end-0 rounded-0 text-white">
                                ДОДАТИ АНОНС
                            </Button>
                        </LinkContainer>
                    </div>
                    {/* 
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