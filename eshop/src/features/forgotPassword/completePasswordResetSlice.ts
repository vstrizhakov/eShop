import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface CompletePasswordResetState {
    isError?: boolean,
};

const sliceName = "completePasswordReset";

const initialState: CompletePasswordResetState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setIsError: (state: CompletePasswordResetState, action: PayloadAction<boolean>) => {
            state.isError = action.payload;
        },
    },
});

export const {
    setIsError,
} = slice.actions;

export default slice.reducer;