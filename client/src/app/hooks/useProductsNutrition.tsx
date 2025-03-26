import {useAppDispatch, useAppSelector} from "../store/store.ts";
import {useEffect} from "react";
import {
    fetchCategories,
    fetchProductsAsync,
    productNutritionSelectors
} from "../../features/productNutrition/productNutritionSlice.ts";

export default function useProductsNutrition() {

    const products = useAppSelector(productNutritionSelectors.selectAll);
    const { productsLoaded, categoriesLoaded, categories, metaData } = useAppSelector(state => state.productNutrition);
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (!productsLoaded) dispatch(fetchProductsAsync());
    }, [dispatch, productsLoaded])

    useEffect(() => {
        if (!categoriesLoaded) dispatch(fetchCategories());
    }, [dispatch, categoriesLoaded])

    return {
        products,
        categories,
        productsLoaded,
        categoriesLoaded,
        metaData
    }
}