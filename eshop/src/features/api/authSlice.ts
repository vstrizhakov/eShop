import { apiSlice } from "./apiSlice";

interface SignUpRequest {
    firstName: string,
    lastName: string,
    email: string,
    phoneNumber: string,
    password: string,
};

interface SignUpResponse {
    succeeded: boolean,
};

interface SignInRequest {
    username: string,
    password: string,
    remember?: boolean,
    returnUrl: string,
};

interface SignInResponse {
    succeeded: boolean,
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

export const authSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        signUp: builder.mutation<SignUpResponse, SignUpRequest>({
            query: request => ({
                url: "/auth/signUp",
                method: "POST",
                body: request,
            }),
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
    }),
});

export const {
    useSignUpMutation,
    useSignInMutation,
    useTrySignOutQuery,
    useSignOutMutation,
} = authSlice;