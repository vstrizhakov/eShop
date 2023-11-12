import { apiSlice } from "./apiSlice";

interface SignUpRequest {
    firstName: string,
    lastName: string,
    phoneNumber: string,
    password: string,
};

interface SignUpResponse {
    succeeded: boolean,
};

interface SignInInfo {
    waitingForConfirmation?: boolean,
};

interface SignInRequest {
    phoneNumber: string,
    password: string,
    remember?: boolean,
    returnUrl: string,
};

interface SignInResponse {
    succeeded: boolean,
    confirmationRequired?: boolean,
    validReturnUrl?: string,
};

interface PostSignOutInfo {
    iframeUrl?: string,
    redirectUrl?: string,
};

interface SignOutInfo {
    prompt: boolean | null,
    postInfo?: PostSignOutInfo,
};

interface ConfirmationLinks {
    telegram: string,
    viber: string,
};

export interface CheckConfirmationRequest {
    returnUrl?: string,
};

export interface CheckConfirmationResponse {
    confirmed: boolean,
    links?: ConfirmationLinks,
    validReturnUrl?: string,
};

export const authSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        signUp: builder.mutation<SignUpResponse, SignUpRequest>({
            query: request => ({
                url: "/auth/signUp",
                method: "POST",
                body: request,
            }),
        }),
        getSignInInfo: builder.query<SignInInfo, unknown>({
            query: () => "/auth/signIn",
        }),
        signIn: builder.mutation<SignInResponse, SignInRequest>({
            query: request => ({
                url: "/auth/signIn",
                method: "POST",
                body: request,
            }),
        }),
        trySignOut: builder.query<SignOutInfo, string>({
            query: logoutId => ({
                url: `/auth/signOut`,
                params: {
                    logoutId: logoutId,
                },
            }),
        }),
        signOut: builder.mutation<SignOutInfo, string>({
            query: logoutId => ({
                url: `/auth/signOut`,
                method: "POST",
                params: {
                    logoutId: logoutId,
                },
            }),
        }),
        checkConfirmation: builder.mutation<CheckConfirmationResponse, CheckConfirmationRequest>({
            query: request => ({
                url: "/auth/checkConfirmation",
                method: "POST",
                body: request,
            }),
        }),
    }),
});

export const {
    useSignUpMutation,
    useCheckConfirmationMutation,
    useGetSignInInfoQuery,
    useSignInMutation,
    useTrySignOutQuery,
    useSignOutMutation,
} = authSlice;