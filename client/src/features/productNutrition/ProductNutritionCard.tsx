import {
    Button,
    Card,
    CardActions,
    CardContent,
    CardMedia,
    Typography
} from "@mui/material";
import { Link } from 'react-router-dom';
import { ProductNutrition } from "../../app/models/productNutrition";

interface Properties {
    product: ProductNutrition;
}

export default function ProductNutritionCard({ product }: Properties) {
    return (
        <Card sx={{ maxWidth: 250, p: 1, textAlign: 'center' }}>
            <Typography
                variant="subtitle1"
                fontWeight={600}
                sx={{ mb: 1 }}
            >
                {product.name}
            </Typography>

            <CardMedia
                component="img"
                image="/images/TEST_PICTURE.jpg"
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
                    {product.calories} ккал · Б: {product.protein}г · Ж: {product.fat}г · В: {product.carbohydrates}г
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
                    Детальніше
                </Button>
            </CardActions>
        </Card>
    );
}
