import {createAsyncThunk, createEntityAdapter, createSlice} from "@reduxjs/toolkit";
import apiClient from "../../app/axios/apiClient.ts";
import {RootState} from '../../app/store/store.ts';
import {MetaData} from '../../app/models/pagination';
import {ProductNutrition, ProductNutritionCategory, ProductNutritionParams} from "../../app/models/productNutrition.ts";

export enum ProductStatus {
    Idle = 'idle',
    Loading = 'loading',
    Error = 'error'
}

interface ProductNutritionState {
    productsLoaded: boolean;
    categoriesLoaded: boolean;
    status: ProductStatus;
    categories: ProductNutritionCategory[];
    productParams: ProductNutritionParams;
    metaData: MetaData | null;
}

const adapter = createEntityAdapter<ProductNutrition>();

function getParamsForServer(productParams: ProductNutritionParams) {
    const {pageNumber, pageSize, orderBy, searchTerm, categories} = productParams;

    const params = new URLSearchParams({
        pageNumber: pageNumber.toString(),
        pageSize: pageSize.toString(),
        orderBy,
    });

    if (searchTerm) params.append('searchTerm', searchTerm);
    if (categories.length)
        params.append(
            'productNutritionCategory',
            categories.map((c) => c.name).join(',')
        );

    return params;
}

export const fetchProductsAsync = createAsyncThunk<ProductNutrition[], void, { state: RootState }>(
    'productNutrition/fetchProductsAsync',
    async (_, thunkAPI) => {
        const params = getParamsForServer(thunkAPI.getState().productNutrition.productParams);
        try {
            const response = await apiClient.ProductNutrition.list(params);
            thunkAPI.dispatch(setMetaData(response.metaData));
            return response.items;
        } catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const fetchProductAsync = createAsyncThunk<ProductNutrition, number>(
    'productNutrition/fetchProductAsync',
    async (productId, thunkAPI) => {
        try {
            return await apiClient.ProductNutrition.getProductById(productId);
        } catch (error: any) {
            return thunkAPI.rejectWithValue({ error: error.data })
        }
    }
)

export const fetchCategories = createAsyncThunk<ProductNutritionCategory[]>(
    'productNutrition/fetchCategories',
    async (_, thunkAPI) => {
        try {
            return apiClient.ProductNutrition.fetchCategories();
        } catch (error: any) {
            return thunkAPI.rejectWithValue({error: error.message})
        }
    }
)

function initParams(): ProductNutritionParams {
    return {
        pageNumber: 1,
        pageSize: 6,
        orderBy: 'name',
        categories: []
    }
}

export const productNutritionSlice = createSlice({
    name: 'productNutrition',
    initialState: adapter.getInitialState<ProductNutritionState>({
        productsLoaded: false,
        categoriesLoaded: false,
        status: ProductStatus.Idle,
        categories: [],
        productParams: initParams(),
        metaData: null
    }),
    reducers: {
        setProductParams: (state, action) => {
            state.productsLoaded = false;
            state.productParams = {...state.productParams, ...action.payload, pageNumber: 1}
        },
        setPageNumber: (state, action) => {
            state.productsLoaded = false;
            state.productParams = {...state.productParams, ...action.payload}
        },
        setMetaData: (state, action) => {
            state.metaData = action.payload
        },
        resetProductParams: (state) => {
            state.productParams = initParams();
        },
        setProduct: (state, action) => {
            adapter.upsertOne(state, action.payload);
            state.productsLoaded = false;
        },
        removeProduct: (state, action) => {
            adapter.removeOne(state, action.payload);
            state.productsLoaded = false;
        }
    },
    extraReducers: (builder => {
        builder.addCase(fetchProductsAsync.fulfilled, (state, action) => {
            adapter.setAll(state, action.payload);
            state.status = ProductStatus.Idle;
            state.productsLoaded = true;
        });
        builder.addCase(fetchCategories.fulfilled, (state, action) => {
            state.categories = action.payload;
            state.status = ProductStatus.Idle;
            state.categoriesLoaded = true;
        });
        builder.addCase(fetchProductAsync.fulfilled , (state, action) => {
            adapter.upsertOne(state, action.payload);
            state.status = ProductStatus.Idle;
        })
        builder.addMatcher(
            (action) => action.type.endsWith('/pending'),
            (state) => {
                state.status = ProductStatus.Loading;
            }
        );
        builder.addMatcher(
            (action) => action.type.endsWith('/rejected'),
            (state, action) => {
                console.error('Rejected:', action);
                state.status = ProductStatus.Error;
            });
    })
})

export const {
    setProductParams,
    resetProductParams,
    removeProduct,
    setMetaData,
    setPageNumber,
    setProduct
} = productNutritionSlice.actions;

export const productNutritionSelectors = adapter.getSelectors((state: RootState) => state.productNutrition);