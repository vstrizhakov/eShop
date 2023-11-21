import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ErrorCode } from "../api/apiSlice";

interface ResetPasswordResult {
    isRequested: boolean,
    error?: ErrorCode,
};

interface RequestPasswordResetState {
    result?: ResetPasswordResult,
};

const sliceName = "requestPasswordReset";

const initialState: RequestPasswordResetState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setResult: (state: RequestPasswordResetState, action: PayloadAction<ResetPasswordResult>) => {
            state.result = action.payload;
        },
        reset: () => initialState,
    },
});

export const {
    setResult,
    reset,
} = slice.actions;

export default slice.reducer;