import React, { useCallback, useState } from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Anchor, Col, Row } from "react-bootstrap";
import Invitation from "./Invitation";
import Compositions from "./Compositions";
import CreateComposition from "./CreateComposition";
import CreateCurrency from "./CreateCurrency";
import Clients from "./Clients";
import { Composition } from "../api/catalogSlice";
import CompositionComponent from "./Composition";

const Main: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
        signIn,
    } = props;

    const [selectedComposition, setSelectedComposition] = useState<Composition | undefined>();

    const onCompositionSelected = useCallback((composition: Composition) => {
        setSelectedComposition(composition);
    }, [setSelectedComposition]);

    return (
        <>
            <div>Main</div>
            {isAuthenticated ? (
                <>
                    <Clients />
                    <Invitation />
                    <CreateCurrency />
                    <Row>
                        <Col>
                            <Compositions onCompositionSelected={onCompositionSelected}/>
                            {selectedComposition && (
                                <CompositionComponent composition={selectedComposition}/>
                            )}
                        </Col>
                        <Col>
                            <CreateComposition />
                        </Col>
                    </Row>
                </>
            ) : (
                <Anchor onClick={signIn}>Sign In</Anchor>
            )}
        </>
    );
};

export default withAuth(Main);