import React from "react";
import { Navbar, Container, Button, Dropdown } from "react-bootstrap";
import { AuthContextProps } from "./auth/authContext";
import { withAuth } from "./auth/withAuth";
import DropdownAnchorToggle from "../components/DropdownAnchorToggle";

const Navigation: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
        signIn,
    } = props;

    return (
        <Navbar>
            <Container >
                <Navbar.Brand>eShop</Navbar.Brand>
                <Navbar.Toggle />
                <Navbar.Collapse className="justify-content-end">
                    {isAuthenticated ? (
                        <Dropdown>
                            <Dropdown.Toggle as={DropdownAnchorToggle} className="text-decoration-none text-resetXZ">
                                Володимир Стрижаков
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                <Dropdown.Item>1unique</Dropdown.Item>
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