import ProductNutritionList from './ProductNutritionList.tsx';
import { useAppDispatch, useAppSelector } from '../../app/store/store.ts';
import { Paper, SpeedDial, SpeedDialAction, SpeedDialIcon} from '@mui/material';
import Grid from '@mui/material/Grid2';
import ProductNutritionSearch from './ProductNutritionSearch.tsx';
import RadioButtonGroupComponent from '../../app/components/RadioButtonGroupComponent.tsx';
import PaginationComponent from '../../app/components/PaginationComponent.tsx';
import useProductsNutrition from "./useProductsNutrition.tsx";
import CheckboxComponent from "../../app/components/CheckboxComponent.tsx";
import {ProductNutritionCategory} from "../../app/models/productNutrition.ts";
import {setPageNumber, setProductParams} from "./productNutritionSlice.ts";
import AddIcon from '@mui/icons-material/Add';
import {useState} from "react";
import ProductNutritionForm from "./ProductNutritionForm.tsx";
import 'simplebar-react/dist/simplebar.min.css';
import CategoryIcon from '@mui/icons-material/Category';
import MenuIcon from '@mui/icons-material/Menu';
import Typography from "@mui/material/Typography";
import ProductCategoryManagement from "./ProductCategoryManagement.tsx";

const sortOptions = [
    { value: 'name', label: 'Від А до Я' },
    { value: 'calories', label: 'Калорії - найменше' },
    { value: 'caloriesDesc', label: 'Калорії - найбільше' },
    {value: 'protein', label: 'Білки - найменше'},
    {value: 'proteinDesc', label: 'Білки - найбільше'},
    {value: 'fat', label: 'Жири - найменше'},
    {value: 'fatDesc', label: 'Жири - найбільше'},
    {value: 'carbohydrates', label: 'Вуглеводи - найменше'},
    {value: 'carbohydratesDesc', label: 'Вуглеводи - найбільше'}
]

export default function ProductNutrition() {
    const {user} = useAppSelector(state => state.account)
    const {products ,categories, metaData} = useProductsNutrition();
    const { productParams } = useAppSelector(state => state.productNutrition);
    const [createMode, setCreateMode] = useState(false);
    const dispatch = useAppDispatch();
    const [categoryManagementMode, setCategoryManagementMode] = useState(false);


    function cancelCreate() {
        setCreateMode(false);
    }

    function cancelCategoryManagement() {
        setCategoryManagementMode(false);
    }

    if (createMode && (!user?.roles?.includes('Admin'))) {
        setCreateMode(false);
    }

    if (categoryManagementMode && (!user?.roles?.includes('Admin'))) {
        setCategoryManagementMode(false);
    }

    if (createMode) return <ProductNutritionForm cancelCreate={cancelCreate}/>
    if (categoryManagementMode) return <ProductCategoryManagement onClose={cancelCategoryManagement}/>;


    // if (!productsLoaded) return <LoadingComponent message='Завантаження продуктів...' />

    return (
        <>
        <Grid container spacing={4}>
            <Grid size={{xs: 3}}>
                <Paper sx={{ mb: 2, borderRadius: 3, boxShadow: 3 }}>
                    <ProductNutritionSearch />
                </Paper>
                <Paper sx={{ p: 2, mb: 2, borderRadius: 3, boxShadow: 3 }}>
                    <Typography variant="subtitle2" fontWeight="bold" mb={1}>
                        Сортування за:
                    </Typography>
                    <RadioButtonGroupComponent
                        selectedValue={productParams.orderBy}
                        options={sortOptions}
                        onChange={(e) => dispatch(setProductParams({ orderBy: e.target.value }))}
                    />
                </Paper>
                <Paper sx={{ p: 2, maxHeight: 400, overflow: 'hidden', borderRadius: 3, boxShadow: 3 }}>
                    <CheckboxComponent
                        availableItems={Array.isArray(categories) ? categories : []}
                        selectedItems={productParams.categories}
                        onChangeSelection={(items: ProductNutritionCategory[]) => dispatch(setProductParams({ categories: items }))}
                    />
                </Paper>
            </Grid>
            <Grid size={{xs: 9}}>
                <ProductNutritionList products={products} />
            </Grid>
            <Grid size={{xs: 3}}/>
            <Grid>
                {metaData && typeof metaData === 'object' && (
                    <PaginationComponent
                        metaData={metaData}
                        onPageChange={(page: number) => dispatch(setPageNumber({ pageNumber: page }))}
                    />
                )}
            </Grid>
            {user?.roles?.includes('Admin') && (
                <SpeedDial
                    ariaLabel="Опції додавання"
                    sx={{ position: 'fixed', bottom: 32, right: 32 }}
                    icon={<SpeedDialIcon openIcon={<MenuIcon />} />}
                >
                    <SpeedDialAction
                        icon={<AddIcon />}
                        tooltipTitle="Додати продукт"
                        onClick={() => setCreateMode(true)}
                    />
                    <SpeedDialAction
                        icon={<CategoryIcon />}
                        tooltipTitle="Керування категоріями"
                        onClick={() => setCategoryManagementMode(true)}
                    />
                </SpeedDial>
            )}

        </Grid>
        </>
    )
}