import {Button, Divider, Table, TableBody, TableCell, TableContainer, TableRow, Typography} from '@mui/material';
import Grid from "@mui/material/Grid2";
import {useNavigate, useParams} from 'react-router-dom';
import {useAppDispatch, useAppSelector} from '../../app/store/store.ts';
import {useEffect} from "react";
import {fetchProductAsync, productNutritionSelectors} from "./productNutritionSlice.ts";
import LoadingComponent from "../../app/layout/LoadingComponent.tsx";

export default function ProductNutritionDetails() {
    const {id} = useParams<{ id: string }>();
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const productNutrition = useAppSelector(state => productNutritionSelectors.selectById(state, +id!));

    useEffect(() => {
        console.log(productNutrition)
        if (!productNutrition && id) dispatch(fetchProductAsync(parseInt(id)))
    }, [id, productNutrition, dispatch]);

    if (!productNutrition) return <LoadingComponent/>

    return (
        <Grid container spacing={6}>
            <Grid size={{xs: 6}}>
                <img src="/public/images/TEST_PICTURE.jpg" alt={productNutrition.name} style={{width: '100%'}}/>
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
                                <TableCell>{productNutrition.calories}</TableCell>
                            </TableRow>
                            <TableRow>
                                <TableCell>Поживні речовини (БЖУ): </TableCell>
                                <TableCell>{productNutrition.protein}</TableCell>
                                <TableCell>{productNutrition.fat}</TableCell>
                                <TableCell>{productNutrition.carbohydrates}</TableCell>
                            </TableRow>
                        </TableBody>
                    </Table>
                </TableContainer>
                <Divider sx={{mb: 2}}/>
                <Button onClick={() => navigate('/productNutrition')}>Назад до продуктів</Button>
            </Grid>
        </Grid>
    )
}