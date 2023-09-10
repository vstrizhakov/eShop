import React from "react";
import { Navbar, Container, Button, Dropdown } from "react-bootstrap";
import { AuthContextProps } from "./auth/authContext";
import { withAuth } from "./auth/withAuth";
import DropdownAnchorToggle from "../components/DropdownAnchorToggle";
import { LinkContainer } from "react-router-bootstrap";

const Navigation: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
        signIn,
    } = props;

    return (
        <Navbar>
            <Container>
                <LinkContainer to="/">
                    <Navbar.Brand>eShop</Navbar.Brand>
                </LinkContainer>
                <Navbar.Toggle />
                <Navbar.Collapse className="justify-content-end">
                    {isAuthenticated ? (
                        <Dropdown>
                            <Dropdown.Toggle as={DropdownAnchorToggle} className="text-decoration-none text-reset">
                                Володимир Стрижаков
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                <LinkContainer to="/clients">
                                    <Dropdown.Item>Клієнти</Dropdown.Item>
                                </LinkContainer>
                            </Dropdown.Menu>
                        </Dropdown>
                    ) : (
                        <Button onClick={signIn} size="sm">
                            Sign In
                        </Button>
                    )}
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default withAuth(Navigation);