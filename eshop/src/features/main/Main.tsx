import React from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Anchor, Col, Row } from "react-bootstrap";
import Invitation from "./Invitation";
import Compositions from "./Compositions";
import CreateComposition from "./CreateComposition";
import CreateCurrency from "./CreateCurrency";

const Main: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
        signIn,
    } = props;

    return (
        <>
            <div>Main</div>
            {isAuthenticated ? (
                <>
                    <Invitation />
                    <CreateCurrency/>
                    <Row>
                        <Col>
                            <Compositions />
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