import { apiSlice } from "./apiSlice";

interface Category {
    id: string,
    name: string,
};

export interface CreateCategoryRequest {
    name: string,
};

interface Currency {
    id: string,
    name: string,
};

export interface CreateCurrencyRequest {
    name: string,
};

interface Product {
    id: string,
    name: string,
    url: string,
};

export interface Composition {
    id: string,
    name: string,
    images: string[],
    products: Product[],
    distributionId?: string,
};

interface ProductPrice {
    currencyId: string,
    price: number,
    discountedPrice?: number,
};

export interface CreateProductRequest {
    name: string,
    url: string,
    price: ProductPrice,
    description: string,
};

export interface CreateCompositionRequest {
    shopId: string,
    image: File,
    products: CreateProductRequest[],
};

interface Shop {
    id: string,
    name: string,
}

function isKey<T extends object>(x: T, k: PropertyKey): k is keyof T {
    return k in x;
}

const fillFormData = <T extends object>(formData: FormData, prefix: string, item: T) => {
    for (const property in item) {
        if (isKey(item, property)) {
            const value = item[property];
            const key = `${prefix}.${property}`;
            if (value) {
                if (!(value instanceof Object)) {
                    formData.append(key, value as any);
                } else {
                    fillFormData(formData, key, value);
                }
            }
        }
    }
};

export const catalogSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getCompositions: builder.query<Composition[], unknown>({
            query: () => "/catalog/compositions",
            providesTags: ["compositions"],
        }),
        createComposition: builder.mutation<Composition, CreateCompositionRequest>({
            query: request => {
                const formData = new FormData();
                formData.append("image", request.image);

                const products = request.products;
                for (let index = 0; index < products.length; index++) {
                    const product = products[index];

                    fillFormData(formData, `products[${index}]`, product);
                }

                formData.append("shopId", request.shopId);

                return {
                    url: "/catalog/compositions",
                    method: "POST",
                    body: formData,
                };
            },
            invalidatesTags: ["compositions"],
        }),
        getComposition: builder.query<Composition, string>({
            query: compositionId => `/catalog/compositions/${compositionId}`,
            providesTags: ["compositions"],
        }),

        getCategories: builder.query<Category[], unknown>({
            query: () => "/catalog/categories",
            providesTags: ["categories"],
        }),
        createCategory: builder.mutation<Category, CreateCategoryRequest>({
            query: request => ({
                url: "/catalog/categories",
                method: "POST",
                body: request,
            }),
            invalidatesTags: ["categories"],
        }),

        getCurrencies: builder.query<Currency[], unknown>({
            query: () => "/catalog/currencies",
            providesTags: ["currencies"],
        }),
        createCurrency: builder.mutation<Currency, CreateCurrencyRequest>({
            query: request => ({
                url: "/catalog/currencies",
                method: "POST",
                body: request,
            }),
            invalidatesTags: ["currencies"],
        }),

        getShops: builder.query<Shop[], unknown>({
            query: () => "/catalog/shops",
            providesTags: ["shops"],
        }),
    }),
});

export const {
    useGetCompositionsQuery,
    useCreateCompositionMutation,
    useGetCompositionQuery,
    useGetCurrenciesQuery,
    useCreateCurrencyMutation,
    useGetShopsQuery,
} = catalogSlice;