import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { apiSlice } from "../features/api/apiSlice";
import authReducer from "../features/auth/authSlice";
import signInReducer from "../features/signIn/signInSlice";
import viewAnnounceReducer from "../features/announces/view/viewAnnounceSlice";

const store = configureStore({
    reducer: {
        [apiSlice.reducerPath]: apiSlice.reducer,
        auth: authReducer,
        signIn: signInReducer,
        viewAnnounce: viewAnnounceReducer,
    },
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware().concat(apiSlice.middleware),
});

export type RootState = ReturnType<typeof store.getState>;

export default store;