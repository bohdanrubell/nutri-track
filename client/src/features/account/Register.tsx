import Grid from '@mui/material/Grid2';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import {
    Box,
    Paper,
    Button,
    IconButton,
    Dialog,
    DialogTitle, DialogContent,
    CircularProgress
} from "@mui/material";
import {Link, useNavigate} from "react-router-dom";
import {useForm} from "react-hook-form";
import apiClient from "../../app/axios/apiClient.ts";
import {toast} from "react-toastify";
import dayjs from 'dayjs';
import {RegisterFormData} from "../../app/models/registerFormData.ts"
import {useEffect, useState} from "react";
import {ActivityLevel, GoalType} from "../../app/models/profileHelpers.ts";
import HelpOutlineIcon from '@mui/icons-material/HelpOutline';
import {registerSchema} from "../../validationSchemas/registerSchema.ts";
import InputComponent from "../../app/components/InputComponent.tsx";
import {zodResolver} from "@hookform/resolvers/zod";
import ListComponent from "../../app/components/ListComponent.tsx";
import DatePickerComponent from "../../app/components/DatePickerComponent.tsx";
import {useDebounce} from "../../app/hooks/useDebounce.ts";

export default function Register() {
    const [openDialog, setOpenDialog] = useState(false);
    const handleOpenDialog = () => setOpenDialog(true);
    const handleCloseDialog = () => setOpenDialog(false);
    const [goals, setGoals] = useState<GoalType[]>([]);
    const [activities, setActivities] = useState<ActivityLevel[]>([]);
    const [isChecking, setIsChecking] = useState(false);
    const [availability, setAvailability] = useState<{isEmailAvailable: boolean, isUsernameAvailable: boolean} | null>(null);

    const navigate = useNavigate();
    const {control, handleSubmit, watch, formState: {isValid}} = useForm<RegisterFormData>({
        mode: 'all',
        resolver: zodResolver(registerSchema)
    });

    const email = watch('email');
    const username = watch('username');
    const debouncedEmail = useDebounce(email, 500);
    const debouncedUsername = useDebounce(username, 500);

    useEffect(() => {
        const checkAvailability = async () => {
            if (!debouncedEmail || !debouncedUsername) return;
            
            setIsChecking(true);
            try {
                const result = await apiClient.Account.checkAvailability(debouncedEmail, debouncedUsername);
                setAvailability(result);
            } catch (error) {
                console.error('Error checking availability:', error);
            } finally {
                setIsChecking(false);
            }
        };

        checkAvailability();
    }, [debouncedEmail, debouncedUsername]);

    useEffect(() => {
        apiClient.Account.getGoalTypes().then(setGoals).catch(error => console.log(error));
        apiClient.Account.getActivityLevels().then(setActivities).catch(error => console.log(error));
    }, []);

    return (
        <>
            <Container component={Paper} maxWidth="md" sx={{display: 'flex', flexDirection: 'column', alignItems: 'center', p: 1, boxShadow:3, borderRadius:3}}>
                <Typography component="h1" variant="h5">Реєстрація користувача</Typography>
                <Box component="form" onSubmit={handleSubmit((data) => {
                    const formattedData = {
                        ...data,
                        dateOfBirth: dayjs(data.dateOfBirth).format('YYYY-MM-DD')
                    };
                    apiClient.Account.register(formattedData)
                        .then(() => {
                            toast.success('Регістрація успішна! Будь ласка, авторизуйтесь!');
                            navigate('/login');
                        });
                })} noValidate sx={{mt: 1}}>
                    <Grid container spacing={2} columns={16}>
                        <Grid size={8}>
                            <InputComponent
                                control={control}
                                label="Ім'я"
                                name="username"
                                helperText={
                                    isChecking ? 'Перевірка...' :
                                    availability && !availability.isUsernameAvailable ? 'Це ім\'я вже зайняте' :
                                    availability && availability.isUsernameAvailable ? 'Ім\'я доступне' : ''
                                }
                                error={!!(availability && !availability.isUsernameAvailable)}
                            />
                            <InputComponent
                                control={control}
                                label="Пароль"
                                type='password'
                                name="password" />
                            <InputComponent
                                control={control}
                                label="Електрона пошта"
                                name="email"
                                helperText={
                                    isChecking ? 'Перевірка...' :
                                    availability && !availability.isEmailAvailable ? 'Ця пошта вже зареєстрована' :
                                    availability && availability.isEmailAvailable ? 'Пошта доступна' : ''
                                }
                                error={!!(availability && !availability.isEmailAvailable)}
                            />
                            <ListComponent
                                control={control}
                                label="Стать"
                                items={['Чоловік','Жінка']}
                                name="gender"
                            />
                            <DatePickerComponent
                                control={control}
                                label="Дата народження"
                                name="dateOfBirth"
                            />
                        </Grid>
                        <Grid size={8}>
                            <InputComponent
                                control={control}
                                label="Зріст"
                                type="number"
                                name="height"/>
                            <InputComponent
                                control={control}
                                label="Вага"
                                type="number"
                                name="weight"/>
                            <ListComponent
                                control={control}
                                label="Ціль"
                                items={goals.map(g => g.name)}
                                name="goal"
                            />
                            <ListComponent
                                control={control}
                                label="Рівень активності"
                                items={activities.map(a => a.name)}
                                name="activity"
                            />
                            <IconButton size="small" onClick={handleOpenDialog}>
                                <HelpOutlineIcon fontSize="small" />
                                <Typography variant="subtitle1">
                                    Рекомендації щодо вибору рівня активності
                                </Typography>
                            </IconButton>
                        </Grid>
                    </Grid>
                    <Button
                        disabled={!isValid || !!(availability && (!availability.isEmailAvailable || !availability.isUsernameAvailable))}
                        type="submit"
                        variant="outlined"
                        sx={{
                            mt: 3,
                            mb: 2,
                            mx: 'auto',
                            display: 'block'
                        }}
                    >
                        {isChecking ? <CircularProgress size={24} /> : 'Зареєструватись'}
                    </Button>
                    <Grid container direction="row" sx={{justifyContent: "center", alignItems: "center"}}>
                        <Grid>
                            <Link to='/login'>
                                {"Маєте аккаунт? Авторизуйтесь"}
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Container>
            <Dialog open={openDialog} onClose={handleCloseDialog}>
                <DialogTitle>Пояснення рівнів активності</DialogTitle>
                <DialogContent>
                    <ul style={{ paddingLeft: '20px', marginTop: '8px' }}>
                        <li><Typography variant="body2">Сидячий спосіб життя: мало чи зовсім немає вправ</Typography></li>
                        <li><Typography variant="body2">Легка активність: легкі вправи 1-3 рази на тиждень</Typography></li>
                        <li><Typography variant="body2">Помірна активність: фізичне навантаження 3-5 разів на тиждень</Typography></li>
                        <li><Typography variant="body2">Висока активність: спорт 6-7 разів на тиждень</Typography></li>
                        <li><Typography variant="body2">Дуже активний: вправи двічі на день або більше</Typography></li>
                    </ul>
                </DialogContent>
            </Dialog>
        </>
    )
}
