import React, { Suspense } from 'react';
import './App.css';
import {
    createBrowserRouter,
    RouterProvider,
} from "react-router-dom";
import SignIn from './features/signIn/SignIn';
import { Container } from 'react-bootstrap';
import SignUp from './features/signUp/SignUp';
import SignOut from './features/signOut/SignOut';

const Main = React.lazy(() => import('./features/main/Main'));

const router = createBrowserRouter([
    {
        path: "/auth/signIn",
        element: <SignIn />,
    },
    {
        path: "/auth/signIn/callback",
        element: <div>Signing you in...</div>
    },
    {
        path: "/auth/signUp",
        element: <SignUp />,
    },
    {
        path: "/auth/signOut",
        element: <SignOut />,
    },
    {
        path: "/auth/signOut/callback",
        element: <div>Signing you out...</div>
    },
    {
        path: "/*",
        element: <Main />,
    },
]);

const App: React.FC = () => {
    return (
        <Container>
            <Suspense>
                <RouterProvider router={router} />
            </Suspense>
        </Container>
    );
}

export default App;
