import {
    Button,
    Dialog, DialogActions, DialogContent, DialogTitle,
    Divider,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow,
    Typography
} from '@mui/material';
import Grid from "@mui/material/Grid2";
import {useNavigate, useParams} from 'react-router-dom';
import {useAppDispatch, useAppSelector} from '../../app/store/store.ts';
import {useEffect, useState} from "react";
import {fetchProductAsync, productNutritionSelectors, removeProduct} from "./productNutritionSlice.ts";
import LoadingComponent from "../../app/layout/LoadingComponent.tsx";
import api from "../../app/api/api.ts";
import {toast} from "react-toastify";
import {ProductNutrition} from "../../app/models/productNutrition.ts";
import ProductNutritionForm from "./ProductNutritionForm.tsx";
import {Edit} from "@mui/icons-material";

export default function ProductNutritionDetails() {
    const {user} = useAppSelector(state => state.account)
    const {id} = useParams<{ id: string }>();
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const productNutrition = useAppSelector(state => productNutritionSelectors.selectById(state, +id!));
    const [deleteMode, setDeleteMode] = useState(false);
    const [updateMode, setUpdateMode] = useState(false);
    const [selectedProductNutrition, setSelectedProductNutrition] = useState<ProductNutrition | undefined>(undefined);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if ((!productNutrition || updateMode) && id) dispatch(fetchProductAsync(parseInt(id)))
    }, [id, productNutrition, dispatch, updateMode]);


    function handleSelectProduct(product: ProductNutrition) {
        setSelectedProductNutrition(product);
        setUpdateMode(true);
    }

    function cancelEdit() {
        if (selectedProductNutrition) setSelectedProductNutrition(undefined);
        setUpdateMode(false);
    }

    const handleSubmit = async () => {
        setLoading(true);
        try {
            await api.Admin.deleteProductNutrition(productNutrition.id);
            setDeleteMode(false);
            dispatch(removeProduct(productNutrition))
            toast.success(`Продукт ${productNutrition.name} успішно видалений із бази даних!`)
        } catch {
            toast.error("Помилка при видалені продукту із бази даних!");
        } finally {
            setLoading(false);
        }
        navigate('/productNutrition')
    };

    if (updateMode) return <ProductNutritionForm productNutrition={productNutrition} cancelCreate={cancelEdit} />
    if (!productNutrition) return <LoadingComponent/>

    return (
        <>
        <Grid container spacing={6}>
            <Grid size={{xs: 6}}>
                <img src={productNutrition.imageId
                    ? productNutrition.imageId
                    : "/images/TEST_PICTURE.jpg"} alt={productNutrition.name} style={{width: '80%'}}/>
            </Grid>
            <Grid size={{xs: 6}}>
                <Typography variant='h3'>{productNutrition.name}</Typography>
                <Divider sx={{mb: 2}}/>
                <TableContainer>
                    <Table>
                        <TableBody sx={{fontSize: '1.1em'}}>
                            <TableRow>
                                <TableCell>Назва продукту</TableCell>
                                <TableCell>{productNutrition.name}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Калорійність на 100 грам: </TableCell>
                                <TableCell>{productNutrition.caloriesPer100Grams}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Поживні речовини (БЖУ): </TableCell>
                                <TableCell>{productNutrition.proteinPer100Grams}</TableCell>
                                <TableCell>{productNutrition.fatPer100Grams}</TableCell>
                                <TableCell>{productNutrition.carbohydratesPer100Grams}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
                <Divider sx={{mb: 2}}/>
                <Button variant='contained' onClick={() => navigate('/productNutrition')}>Назад до бази продуктів</Button>
                {user?.roles?.includes('Admin') && (
                    <>
                        <Button
                            onClick={() => handleSelectProduct(productNutrition)}
                            startIcon={<Edit />}
                        >
                            Редагувати
                        </Button>

                        <Button
                            variant='contained'
                            color='error'
                            onClick={() => setDeleteMode(true)}
                        >
                            Видалити продукт
                        </Button>
                    </>
                )}

            </Grid>
        </Grid>

            <Dialog open={deleteMode} onClose={() => setDeleteMode(false)}>
                <DialogTitle>Видалення</DialogTitle>
                <DialogContent>
                    <Typography>
                        Ви впевнені, що хочете видалити продукт {productNutrition.name} із бази даних?
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteMode(false)} disabled={loading}>
                        Скасувати
                    </Button>
                    <Button
                        variant="contained"
                        onClick={handleSubmit}
                        disabled={loading}
                    >
                        {loading ? 'Видаляється...' : 'Видалити'}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    )
}