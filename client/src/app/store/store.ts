import {configureStore} from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import {accountSlice} from "../../features/account/accountSlice.tsx";
import {productNutritionSlice} from "../../features/productNutrition/productNutritionSlice.ts";

export const store = configureStore({
    reducer: {
        account: accountSlice.reducer,
        productNutrition: productNutritionSlice.reducer
    }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;