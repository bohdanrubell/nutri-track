import ProductNutritionList from './ProductNutritionList.tsx';
import LoadingComponent from '../../app/layout/LoadingComponent';
import { useAppDispatch, useAppSelector } from '../../app/store/store.ts';
import {Fab, Paper} from '@mui/material';
import Grid from '@mui/material/Grid2';
import ProductNutritionSearch from './ProductNutritionSearch.tsx';
import RadioButtonGroup from '../../app/components/RadioButtonGroup';
import AppPagination from '../../app/components/AppPagination';
import useProductsNutrition from "../../app/hooks/useProductsNutrition.tsx";
import CheckboxButton from "../../app/components/CheckboxButton.tsx";
import {ProductNutritionCategory} from "../../app/models/productNutrition.ts";
import {setPageNumber, setProductParams} from "./productNutritionSlice.ts";
import AddIcon from '@mui/icons-material/Add';
import {useState} from "react";
import ProductNutritionForm from "./ProductNutritionForm.tsx";

const sortOptions = [
    { value: 'name', label: 'Від А до Я' },
    { value: 'caloriesDesc', label: 'Калорії - найбільше' },
    { value: 'calories', label: 'Калорії - найменше' },
]

export default function ProductNutrition() {
    const {user} = useAppSelector(state => state.account)
    const {products, categoriesLoaded, categories, metaData} = useProductsNutrition();
    const { productParams } = useAppSelector(state => state.productNutrition);
    const [createMode, setCreateMode] = useState(false);
    const dispatch = useAppDispatch();

    function cancelCreate() {
        setCreateMode(false);
    }

    if (createMode) return <ProductNutritionForm cancelCreate={cancelCreate}/>


    if (!categoriesLoaded) return <LoadingComponent message='Завантаження продуктів...' />

    return (
        <>

        <Grid container spacing={4}>
            <Grid size={{xs: 3}}>
                <Paper sx={{ mb: 2 }}>
                    <ProductNutritionSearch />
                </Paper>
                <Paper sx={{ p: 2, mb: 2 }}>
                    <RadioButtonGroup
                        selectedValue={productParams.orderBy}
                        options={sortOptions}
                        onChange={(e) => dispatch(setProductParams({ orderBy: e.target.value }))}
                    />
                </Paper>
                <Paper sx={{ p: 2 }}>
                    <CheckboxButton
                        items={categories}
                        checked={productParams.categories}
                        onChange={(items: ProductNutritionCategory[]) => dispatch(setProductParams({ categories: items }))}
                    />
                </Paper>
            </Grid>
            <Grid size={{xs: 9}}>
                <ProductNutritionList products={products} />
            </Grid>
            <Grid size={{xs: 3}}/>
            <Grid>
                {metaData &&
                    <AppPagination
                        metaData={metaData}
                        onPageChange={(page: number) => dispatch(setPageNumber({pageNumber: page}))}
                    />}
            </Grid>
            {user!.roles!.includes('Admin') && (
                <Fab
                    color="success"
                    aria-label="add"
                    sx={{
                        position: 'fixed',
                        bottom: 32,
                        right: 32
                    }}
                    onClick={() => {setCreateMode(true)}}
                >
                    <AddIcon />
                </Fab>
            )}
        </Grid>
        </>
    )
}