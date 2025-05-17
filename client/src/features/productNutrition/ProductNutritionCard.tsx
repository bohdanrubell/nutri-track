import {
    Button,
    Card,
    CardActions,
    CardContent,
    CardMedia, Dialog, DialogActions, DialogContent, DialogTitle, Tooltip,
    Typography
} from "@mui/material";
import { Link } from 'react-router-dom';
import { ProductNutrition } from "../../app/models/productNutrition";
import {useState} from "react";
import TextField from "@mui/material/TextField";
import apiClient from "../../app/axios/apiClient.ts";
import {toast} from "react-toastify";
import {useAppSelector} from "../../app/store/store.ts";

interface Properties {
    product: ProductNutrition;
}

export default function ProductNutritionCard({ product }: Properties) {
    const { user } = useAppSelector((state) => state.account);
    const [open, setOpen] = useState(false);
    const [grams, setGrams] = useState('100');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async () => {
        setLoading(true);
        try {
            await apiClient.Diary.addProductRecord({
                productNutritionId: product.id,
                consumedGrams: Number(grams)
            });
            setOpen(false);
            setGrams('100');
            toast.success("Успішно створено новий запис спожитого продукту!")
        } catch {
            toast.error("Помилка при додаванні продукту до щоденнику!");
        } finally {
            setLoading(false);
        }
    };

    const handleClose = () => {
        setOpen(false);
        setGrams('100');
    };


    return (
        <>
            <Card
                sx={{
                    maxWidth: 250,
                    p: 1,
                    textAlign: 'center',
                    borderRadius: 3,
                    boxShadow: 3,
                    transition: 'all 0.3s ease',
                    '&:hover': {
                        boxShadow: 6,
                        transform: 'translateY(-5px)',
                    },
                }}
            >
            <Tooltip title={product.name}>
                <Typography
                    variant="subtitle1"
                    fontWeight={600}
                    sx={{
                        mb: 1,
                        height: 32,
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        whiteSpace: 'nowrap',
                        cursor: 'pointer',
                    }}
                >
                    {product.name}
                </Typography>
            </Tooltip>

            <CardMedia
                component="img"
                image={product.imageId
                    ? product.imageId
                    : "https://res.cloudinary.com/dzvxzhmfr/image/upload/v1747510946/product_without_pic_main.jpg"}
                alt={product.name}
                sx={{
                    height: 100,
                    objectFit: 'contain',
                    bgcolor: 'primary.light',
                    mx: 'auto',
                }}
            />

            <CardContent sx={{ p: 1 }}>
                <Typography variant="caption" color="text.secondary">
                    {product.caloriesPer100Grams} ккал · Б: {product.proteinPer100Grams}г · Ж: {product.fatPer100Grams}г · В: {product.carbohydratesPer100Grams}г
                </Typography>
            </CardContent>

            <CardActions sx={{ p: 1 }}>
                {/* Перша кнопка: Детальніше або Налаштування */}
                <Button
                    component={Link}
                    to={`/productNutrition/${product.id}`}
                    size="small"
                    variant="outlined"
                    color="primary"
                    fullWidth
                >
                    {!user && "Детальніше"}
                    {user?.roles?.includes('User') && "Детальніше"}
                    {user?.roles?.includes('Admin') && "Налаштування"}
                </Button>

                {/* Друга кнопка: Додати тільки для користувачів із роллю User */}
                {user?.roles?.includes('User') && (
                    <Button
                        size="small"
                        variant="contained"
                        color="primary"
                        fullWidth
                        onClick={() => setOpen(true)}
                    >
                        Додати
                    </Button>
                )}
            </CardActions>
        </Card>

            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Скільки грамів бажаєте додати?</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Грами"
                        type="number"
                        fullWidth
                        value={grams}
                        onChange={(e) => {
                            const value = e.target.value;
                            if (value === '' || (/^\d+$/.test(value) && parseInt(value) >= 1)) {
                                setGrams(value);
                            }
                        }}
                        error={grams === '' || Number(grams) < 1 || Number(grams) > 2000}
                        helperText={
                            grams === ''
                                ? 'Поле не може бути порожнім'
                                : Number(grams) < 1
                                    ? 'Грами мають бути більше або дорівнювати 1'
                                    : Number(grams) > 2000
                                        ? 'Грами не можуть перевищувати 2000'
                                        : ''
                        }
                    />


                    <div style={{ marginTop: '16px' }}>
                        <Typography variant="subtitle2" color="text.secondary">
                            Попередні розрахунки:
                        </Typography>
                        <Typography variant="body2">
                            {((product.caloriesPer100Grams * Number(grams)) / 100).toFixed(0)} ккал ·
                            Б: {((product.proteinPer100Grams * Number(grams)) / 100).toFixed(1)}г ·
                            Ж: {((product.fatPer100Grams * Number(grams)) / 100).toFixed(1)}г ·
                            В: {((product.carbohydratesPer100Grams * Number(grams)) / 100).toFixed(1)}г
                        </Typography>
                    </div>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} disabled={loading}>
                        Скасувати
                    </Button>
                    <Button
                    variant="contained"
                    onClick={handleSubmit}
                    disabled={loading || grams === '' || Number(grams) < 1 || Number(grams) > 2000}
                >
                    {loading ? 'Додається...' : 'Додати'}
                </Button>
                </DialogActions>
            </Dialog>

        </>
    );
}
