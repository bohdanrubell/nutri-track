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
import api from "../../app/api/api.ts";
import {toast} from "react-toastify";
import {useAppSelector} from "../../app/store/store.ts";

interface Properties {
    product: ProductNutrition;
}

export default function ProductNutritionCard({ product }: Properties) {
    const { user } = useAppSelector((state) => state.account);
    const [open, setOpen] = useState(false);
    const [grams, setGrams] = useState(100);
    const [loading, setLoading] = useState(false);

    const handleSubmit = async () => {
        setLoading(true);
        try {
            await api.Diary.addProductRecord({
                productNutritionId: product.id,
                consumedGrams: grams
            });
            setOpen(false);
            setGrams(100);
        } catch {
            toast.error("Помилка при додаванні продукту до щоденнику!");
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
        <Card sx={{ maxWidth: 250, p: 1, textAlign: 'center' }}>
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
                    : "/images/TEST_PICTURE.jpg"}
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
                <Button
                    component={Link}
                    to={`/productNutrition/${product.id}`}
                    size="small"
                    variant="outlined"
                    color="primary"
                    fullWidth
                >
                    {user?.roles?.includes('User')
                        ? "Детальніше"
                        : "Налаштування"}
                </Button>
                {user!.roles!.includes('User') && <Button
                    size="small"
                    variant="contained"
                    color="primary"
                    fullWidth
                    onClick={() => setOpen(true)}
                >
                    Додати
                </Button>}
            </CardActions>
        </Card>

            <Dialog open={open} onClose={() => setOpen(false)}>
                <DialogTitle>Скільки грамів бажаєте додати?</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Грами"
                        type="number"
                        fullWidth
                        value={grams}
                        onChange={(e) => setGrams(Number(e.target.value))}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpen(false)} disabled={loading}>
                        Скасувати
                    </Button>
                    <Button
                        variant="contained"
                        onClick={handleSubmit}
                        disabled={loading}
                    >
                        {loading ? 'Додається...' : 'Додати'}
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    );
}
