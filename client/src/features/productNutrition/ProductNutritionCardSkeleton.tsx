import { Card, CardContent, CardActions, Skeleton } from "@mui/material";

export default function ProductNutritionCardSkeleton() {
    return (
        <Card
            sx={{
                maxWidth: 250,
                p: 1,
                textAlign: 'center',
                borderRadius: 3,
                boxShadow: 3,
            }}
        >
            <Skeleton variant="text" width="80%" height={32} sx={{ mb: 1 }} />
            
            <Skeleton 
                variant="rectangular" 
                height={100} 
                sx={{ 
                    bgcolor: 'primary.light',
                    mx: 'auto',
                }} 
            />

            <CardContent sx={{ p: 1 }}>
                <Skeleton variant="text" width="100%" height={20} />
            </CardContent>

            <CardActions sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                <Skeleton variant="rectangular" width="100%" height={36} />
                <Skeleton variant="rectangular" width="100%" height={36} />
            </CardActions>
        </Card>
    );
} 