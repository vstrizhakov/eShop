import React, { Suspense } from 'react';
import './App.css';
import { Outlet } from "react-router-dom";
import './App.scss';
import { Container } from 'react-bootstrap';
import Navigation from './features/Navigation';

const App: React.FC = () => {
    return (
        <>
            <Navigation />
            <Container className="mt-5">
                <Suspense>
                    <Outlet />
                </Suspense>
            </Container>
        </>
    );
}

export default App;
