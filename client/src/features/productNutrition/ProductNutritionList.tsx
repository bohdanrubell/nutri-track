import Grid from "@mui/material/Grid2";
import { useAppSelector } from '../../app/store/store.ts';
import ProductNutritionCard from "./ProductNutritionCard.tsx";
import {ProductNutrition} from "../../app/models/productNutrition.ts";
import LoadingComponent from "../../app/layout/LoadingComponent.tsx";

interface ListProperties {
    products: ProductNutrition[];
}

export default function ProductNutritionList({ products }: ListProperties) {
    const { productsLoaded } = useAppSelector(state => state.productNutrition);
    return (
        <Grid container spacing={4}>
            {products.map(product => (
                <Grid size={{xs: 4}} key={product.id}>
                    {!productsLoaded ? (
                        <LoadingComponent/>
                    ) : (
                        <ProductNutritionCard product={product} />
                    )}
                </Grid>
            ))}
        </Grid>
    )
}