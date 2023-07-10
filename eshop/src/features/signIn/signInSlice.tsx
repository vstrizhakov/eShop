import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface SignInState {
    isError?: boolean,
};

const sliceName = "signIn";

const initialState: SignInState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setIsError: (state: SignInState, action: PayloadAction<boolean>) => {
            state.isError = action.payload;
        },
    },
});

export const {
    setIsError,
} = slice.actions;

export default slice.reducer;