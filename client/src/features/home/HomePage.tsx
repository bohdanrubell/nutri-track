import { Box, Button, Typography, Link } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {useAppSelector} from "../../app/store/store.ts";

export default function HomePage() {
    const navigate = useNavigate();
    const {user} = useAppSelector(state => state.account);
    return (
        <Box
            sx={{
                position: 'relative',
                height: '100vh',
                width: '100%',
                overflow: 'hidden',
            }}
        >
            <img
                src="/images/TEST_PICTURE.jpg"
                alt="background"
                style={{
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    width: '100%',
                    height: '100%',
                    objectFit: 'cover',
                    zIndex: 0,
                }}
            />
            <Box
                sx={{
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    width: '100%',
                    height: '100%',
                    backgroundColor: 'rgba(0, 0, 0, 0.5)',
                    zIndex: 1,
                }}
            />
            <Box
                sx={{
                    position: 'absolute',
                    top: 20,
                    left: 0,
                    right: 0,
                    zIndex: 2,
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    gap: 1,
                }}
            >
                <img
                    src="/images/icon.png"
                    alt="NutriTrack Icon"
                    style={{ width: 40, height: 40 }}
                />
                <Typography variant="h5" fontWeight="bold" color="white">
                    NutriTrack
                </Typography>
            </Box>
            <Box
                sx={{
                    position: 'relative',
                    zIndex: 2,
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    justifyContent: 'center',
                    alignItems: 'center',
                    textAlign: 'center',
                    color: 'white',
                    px: 2,
                }}
            >
                <Typography variant="h4" fontWeight="bold" mb={4}>
                    Зручний веб-застосунок для керування своїм раціоном харчування!
                </Typography>

                <Button
                    variant="contained"
                    size="large"
                    sx={{
                        mb: 2,
                        backgroundColor: '#4CAF50',
                        '&:hover': { backgroundColor: '#45A049' },
                        fontWeight: 'bold'
                    }}
                    onClick={() => {
                        if (user){
                            navigate('/diary')
                        }
                        else{
                            navigate('/register')
                        }
                    }}
                >
                    Почати користуватись
                </Button>

                {!user && (
                    <Typography variant="body2" mt={2}>
                        Маєте акаунт?{' '}
                        <Link href="/login" underline="hover" color="inherit">
                            Авторизуйтесь
                        </Link>
                    </Typography>
                )}
            </Box>
        </Box>
    );
}
