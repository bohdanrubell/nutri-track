import {Box, Button,  Link, Typography} from '@mui/material';
import Grid from '@mui/material/Grid2'
import {useNavigate} from 'react-router-dom';
import {useAppSelector} from "../../app/store/store.ts";
import {motion} from 'framer-motion';
import RestaurantIcon from '@mui/icons-material/Restaurant';
import WhatshotIcon from '@mui/icons-material/Whatshot';
import ShowChartIcon from '@mui/icons-material/ShowChart';


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
                src="https://res.cloudinary.com/dzvxzhmfr/image/upload/v1744540666/home_page.jpg"
                alt="NutriTrack background"
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
                    src="https://res.cloudinary.com/dzvxzhmfr/image/upload/v1747509918/icon.png"
                    alt="NutriTrack Icon"
                    style={{width: 40, height: 40}}
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
                <motion.div
                    initial={{opacity: 0, y: 30}}
                    animate={{opacity: 1, y: 0}}
                    transition={{duration: 1}}
                >
                    <Typography
                        variant="h4"
                        fontWeight="bold"
                        mb={4}
                        sx={{
                            fontSize: {xs: '1.8rem', md: '2.125rem'}, // xs - менший розмір, md - нормальний h4
                        }}
                    >
                        Зручний веб-застосунок для керування своїм раціоном харчування!
                    </Typography>
                    <Button
                        variant="contained"
                        size="large"
                        sx={{
                            mb: 4,
                            backgroundColor: '#4CAF50',
                            '&:hover': {backgroundColor: '#45A049'},
                            fontWeight: 'bold',
                        }}
                        onClick={() => {
                            if (user?.roles?.includes('Admin')) {
                                navigate('/productNutrition');
                            } else if (user?.roles?.includes('User')) {
                                navigate('/diary');
                            } else {
                                navigate('/register');
                            }
                        }}
                    >
                        Почати користуватись
                    </Button>
                </motion.div>
                {!user && (
                    <Typography variant="body2" mt={2}>
                        Маєте акаунт?{' '}
                        <Link href="/login" underline="hover" color="inherit">
                            Авторизуйтесь
                        </Link>
                    </Typography>
                )}

                <Grid container spacing={2} mt={6} justifyContent="center" maxWidth="md">
                    {[
                        {
                            icon: <RestaurantIcon sx={{ fontSize: 50 }} />, // їжа
                            title: "Додавайте продукти легко",
                            desc: "Зберігайте страви та інгредієнти у пару кліків."
                        },
                        {
                            icon: <WhatshotIcon sx={{ fontSize: 50 }} />, // калорії
                            title: "Контролюйте калорії",
                            desc: "Розрахунок добових норм КБЖВ автоматично."
                        },
                        {
                            icon: <ShowChartIcon sx={{ fontSize: 50 }} />, // статистика
                            title: "Статистика прогресу",
                            desc: "Щоденні, тижневі та місячні графіки споживання."
                        }
                    ].map((item, index) => (
                        <Grid size={{xs:12, md:4}} key={index}>
                            <motion.div
                                initial={{ opacity: 0, y: 20 }}
                                animate={{ opacity: 1, y: 0 }}
                                transition={{ duration: 0.8, delay: 1 + index * 0.3 }}
                                style={{ textAlign: 'center' }}
                            >
                                <Box mb={1}>
                                    {item.icon}
                                </Box>
                                <Typography variant="h6" fontWeight="bold" mb={1}>
                                    {item.title}
                                </Typography>
                                <Typography variant="body2">
                                    {item.desc}
                                </Typography>
                            </motion.div>
                        </Grid>
                    ))}
                </Grid>
            </Box>

            <Box
                position="absolute"
                bottom={10}
                width="100%"
                textAlign="center"
                zIndex={2}
            >
                <Typography variant="caption" color="white">
                    © 2025 NutriTrack. Всі права захищено.
                </Typography>
            </Box>
        </Box>
    );
}
