import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { Provider } from 'react-redux';
import store from "./app/store";
import AuthProvider from './features/auth/AuthProvider';
import 'bootstrap-icons/font/bootstrap-icons.css';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import AddAnnounce from './features/announces/add/AddAnonce';
import Clients from './features/main/Clients';
import Invitation from './features/main/Invitation';
import SignIn from './features/signIn/SignIn';
import SignOut from './features/signOut/SignOut';
import SignUp from './features/signUp/SignUp';
import Main from './features/main/Main';
import AnnounceContainer from './features/announces/view/AnnounceContainer';
import Confirm from './features/confirm/Confirm';
import RequestPasswordReset from './features/forgotPassword/RequestPasswordReset';
import CompletePasswordReset from './features/forgotPassword/CompletePasswordReset';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

const router = createBrowserRouter([
    {
        id: "root",
        path: "/",
        Component: App,
        children: [
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
                element: <div>Signing you out...</div>,
            },
            {
                path: "/auth/confirm",
                element: <Confirm />,
            },
            {
                path: "/auth/requestPasswordReset",
                element: <RequestPasswordReset />,
            },
            {
                path: "/auth/completePasswordReset",
                element: <CompletePasswordReset />,
            },
            {
                path: "/addAnnounce",
                element: <AddAnnounce />,
            },
            {
                path: "/clients",
                element: <Clients />,
            },
            {
                path: "/announces/:announceId",
                element: <AnnounceContainer />,
            },
            {
                path: "/",
                element: <Main />,
            },
        ],
    },
    {
        path: "/announcer/:providerId",
        element: <Invitation />,
    },
]);

root.render(
    <React.StrictMode>
        <Provider store={store}>
            <AuthProvider>
                <RouterProvider router={router} />
            </AuthProvider>
        </Provider>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
