import React from "react";
import { Navbar, Container, Button, Dropdown } from "react-bootstrap";
import { AuthContextProps } from "./auth/authContext";
import { withAuth } from "./auth/withAuth";
import DropdownAnchorToggle from "../components/DropdownAnchorToggle";
import { LinkContainer } from "react-router-bootstrap";
import { useLocation } from "react-router-dom";

const Navigation: React.FC<AuthContextProps> = props => {
    const {
        isAuthenticated,
        claims,
        signIn,
        signOut,
    } = props;

    const location = useLocation();

    let name = claims.given_name;
    if (claims.family_name) {
        name += ` ${claims.family_name}`;
    }

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
                                {name}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                <LinkContainer to="/">
                                    <Dropdown.Item>Анонси</Dropdown.Item>
                                </LinkContainer>
                                <LinkContainer to="/clients">
                                    <Dropdown.Item>Клієнти</Dropdown.Item>
                                </LinkContainer>
                                <Dropdown.Item onClick={signOut}>Вийти з акаунту</Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                    ) : (
                        <>
                            {!location.pathname.includes("/auth") && (
                                <Button onClick={signIn} size="sm" className="fw-semibold">
                                    Увійти
                                </Button>
                            )}
                        </>
                    )}
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default withAuth(Navigation);