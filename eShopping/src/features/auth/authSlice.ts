import { createSlice, PayloadAction } from "@reduxjs/toolkit";

const sliceName = "auth";

export interface Claims {
    [claim: string]: any,
};

interface AuthState {
    isAuthenticated?: boolean,
    token?: string,
    claims?: Claims,
};

const initialState: AuthState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setIsAuthenticated: (state: AuthState, action: PayloadAction<boolean>) => {
            state.isAuthenticated = action.payload;
        },
        setToken: (state: AuthState, action: PayloadAction<string>) => {
            state.token = action.payload;
        },
        setClaims: (state: AuthState, action: PayloadAction<Claims>) => {
            state.claims = action.payload;
        },
    },
});

export const {
    setIsAuthenticated,
    setToken,
    setClaims,
} = slice.actions;

export default slice.reducer;