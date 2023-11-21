import { AnyAction, ThunkAction, combineReducers, configureStore } from "@reduxjs/toolkit";
import { apiSlice } from "../features/api/apiSlice";
import authReducer from "../features/auth/authSlice";
import signInReducer from "../features/signIn/signInSlice";
import viewAnnounceReducer from "../features/announces/view/viewAnnounceSlice";
import completePasswordResetReducer from "../features/forgotPassword/completePasswordResetSlice";
import signUpReducer from "../features/signUp/signUpSlice";
import confirmReducer from "../features/confirm/confirmSlice";
import requestPasswordResetReducer from "../features/forgotPassword/requestPasswordResetSlice";

const store = configureStore({
    reducer: {
        [apiSlice.reducerPath]: apiSlice.reducer,
        auth: authReducer,
        signIn: signInReducer,
        signUp: signUpReducer,
        viewAnnounce: viewAnnounceReducer,
        completePasswordReset: completePasswordResetReducer,
        confirm: confirmReducer,
        requestPasswordReset: requestPasswordResetReducer,
    },
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware().concat(apiSlice.middleware),
});

export type RootState = ReturnType<typeof store.getState>;

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  AnyAction
>;

export default store;