import { Container, Paper, Typography, Divider, Button, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function NotFoundComponent() {
    const navigate = useNavigate();

    return (
        <Container component={Paper} sx={{ height: 400, display: 'flex', flexDirection: 'column', justifyContent: 'center', alignItems: 'center', p: 4 }}>
            <Typography gutterBottom variant="h3" align="center">
                Ой! Ми не змогли знайти те, що ви шукаєте.
            </Typography>
            <Divider sx={{ my: 2, width: '100%' }} />
            <Box sx={{ width: '100%', display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="outlined"
                    size="large"
                    onClick={() => navigate(-1)}
                >
                    Повернутися назад
                </Button>
            </Box>
        </Container>
    );
}
