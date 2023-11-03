import React from "react";
import { withAuth } from "../auth/withAuth";
import { AuthContextProps } from "../auth/authContext";
import { Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";

const Main: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
    } = props;

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
                </>
            )}
        </>
    );
};

export default withAuth(Main);