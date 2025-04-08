import {
    Button,
    Dialog, DialogActions, DialogContent, DialogTitle,
    Divider, Stack,
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
import LoadingComponent from "../../app/components/LoadingComponent.tsx";
import apiClient from "../../app/axios/apiClient.ts";
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

    const handleDelete = async () => {
        setLoading(true);
        try {
            await apiClient.Admin.deleteProductNutrition(productNutrition.id);
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
                <Grid size={{xs:12, md:6}} display="flex" justifyContent="center" alignItems="center">
                    <img
                        src={productNutrition.imageId || "/images/TEST_PICTURE.jpg"}
                        alt={productNutrition.name}
                        style={{ width: '80%', maxHeight: 400, objectFit: 'contain' }}
                    />
                </Grid>
                <Grid size={{xs:12, md:6}}>
                    <Typography variant="h4" fontWeight={600} gutterBottom>
                        {productNutrition.name}
                    </Typography>
                    <Divider sx={{ mb: 2 }} />
                    <TableContainer>
                        <Table>
                            <TableBody>
                                <TableRow>
                                    <TableCell sx={{ fontWeight: 500 }}>Калорійність (100 г)</TableCell>
                                    <TableCell>{productNutrition.caloriesPer100Grams} ккал</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{ fontWeight: 500 }}>Білки (Б)</TableCell>
                                    <TableCell>{productNutrition.proteinPer100Grams} г</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{ fontWeight: 500 }}>Жири (Ж)</TableCell>
                                    <TableCell>{productNutrition.fatPer100Grams} г</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{ fontWeight: 500 }}>Вуглеводи (В)</TableCell>
                                    <TableCell>{productNutrition.carbohydratesPer100Grams} г</TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>

                    <Divider sx={{ my: 2 }} />

                    <Stack direction="row" spacing={2} flexWrap="wrap">
                        <Button
                            variant="outlined"
                            onClick={() => navigate('/productNutrition')}
                        >
                            Назад до бази
                        </Button>
                        {user?.roles?.includes('Admin') && (
                            <>
                                <Button
                                    variant="contained"
                                    color="primary"
                                    startIcon={<Edit />}
                                    onClick={() => handleSelectProduct(productNutrition)}
                                >
                                    Редагувати
                                </Button>

                                <Button
                                    variant="contained"
                                    color="error"
                                    onClick={() => setDeleteMode(true)}
                                >
                                    Видалити
                                </Button>
                            </>
                        )}
                    </Stack>
                </Grid>
            </Grid>

            <Dialog open={deleteMode} onClose={() => setDeleteMode(false)}>
                <DialogTitle>Видалення продукту</DialogTitle>
                <DialogContent>
                    <Typography>
                        Ви впевнені, що хочете видалити <strong>{productNutrition.name}</strong> із бази даних?
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteMode(false)} disabled={loading}>
                        Скасувати
                    </Button>
                    <Button
                        variant="contained"
                        color="error"
                        onClick={handleDelete}
                        disabled={loading}
                    >
                        {loading ? 'Видалення...' : 'Видалити'}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}