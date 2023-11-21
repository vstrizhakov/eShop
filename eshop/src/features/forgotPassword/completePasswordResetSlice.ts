import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ErrorCode } from "../api/apiSlice";

interface CompletePasswordResetState {
    error?: ErrorCode,
};

const sliceName = "completePasswordReset";

const initialState: CompletePasswordResetState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setError: (state: CompletePasswordResetState, action: PayloadAction<ErrorCode>) => {
            state.error = action.payload;
        },
    },
});

export const {
    setError,
} = slice.actions;

export default slice.reducer;