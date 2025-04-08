import TextField from '@mui/material/TextField';
import Grid from '@mui/material/Grid2';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import {Box, Paper, FormControl, InputLabel, Select, MenuItem, Button} from "@mui/material";
import {Link, useNavigate} from "react-router-dom";
import {Controller, useForm} from "react-hook-form";
import apiClient from "../../app/axios/apiClient.ts";
import {toast} from "react-toastify";
import dayjs from 'dayjs';
import {DatePicker} from "@mui/x-date-pickers";
import {RegisterFormData} from "../../app/models/registerFormData.ts"
import {useEffect, useState} from "react";
import {ActivityLevel, GoalType} from "../../app/models/profileHelpers.ts";

export default function Register() {
    const [goals, setGoals] = useState<GoalType[]>([]);
    const [activities, setActivities] = useState<ActivityLevel[]>([]);

    const navigate = useNavigate();
    const {register, control, watch, setValue, handleSubmit, setError, formState: {isSubmitting, errors, isValid}} = useForm<RegisterFormData>({
        mode: 'all'
    });

    useEffect(() => {
        apiClient.Account.getGoalTypes().then(setGoals).catch(error => console.log(error));
        apiClient.Account.getActivityLevels().then(setActivities).catch(error => console.log(error));
    }, []);

    function handleApiErrors(errors: any) {
        if (errors) {
            Object.keys(errors).forEach((key) => {
                const errorMessage = errors[key];
                if (key.includes('Password')) {
                    setError('password', {message: errorMessage});
                } else if (key.includes('Email')) {
                    setError('email', {message: errorMessage});
                } else if (key.includes('Username')) {
                    setError('username', {message: errorMessage});
                } else if (key.includes('gender')) {
                    setError('gender', {message: errorMessage});
                } else if (key.includes('date')) {
                    setError('dateOfBirth', {message: errorMessage});
                } else if (key.includes('height')) {
                    setError('height', {message: errorMessage});
                } else if (key.includes('weight')) {
                    setError('weight', {message: errorMessage});
                } else if (key.includes('goal')) {
                    setError('goal', {message: errorMessage});
                } else if (key.includes('activity')) {
                    setError('activity', {message: errorMessage});
                }
            });
        }
    }

    return (
            <Container component={Paper} maxWidth="sm" sx={{display: 'flex', flexDirection: 'column', alignItems: 'center', p: 2}}>
                <Typography component="h1" variant="h5">Реєстрація користувача</Typography>
                <Box component="form" onSubmit={handleSubmit((data) => {
                    const formattedData = {
                        ...data,
                        dateOfBirth: dayjs(data.dateOfBirth).format('YYYY-MM-DD')
                    };
                    apiClient.Account.register(formattedData)
                        .catch((errors) => handleApiErrors(errors))
                        .then(() => {
                            toast.success('Регістрація успішна! Будь ласка, авторизуйтесь!');
                            navigate('/login');
                        });
                })} noValidate sx={{mt: 1}}>
                    <Grid container spacing={2} columns={16}>
                        <Grid size={8}>
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Ім'я"
                                autoFocus
                                {...register('username', {required: 'Імя є обовязковим'})}
                                error={!!errors.username}
                                helperText={errors?.username?.message as string}
                            />
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Пароль"
                                type="password"
                                autoComplete="current-password"
                                {...register('password', {
                                    required: "Пароль є обов'язковим",
                                    pattern: {
                                        value: /(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$/,
                                        message: 'Слабий пароль'
                                    }
                                })}
                                error={!!errors.password}
                                helperText={errors?.password?.message as string}
                            />
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Email"
                                {...register('email', {
                                    required: 'Пошта є обов"язковою',
                                    pattern: {
                                        value: /^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$/,
                                        message: 'Email is not valid'
                                    }
                                })}
                                error={!!errors.email}
                                helperText={errors?.email?.message as string}
                            />
                            <FormControl fullWidth margin="normal" error={!!errors.gender}>
                                <InputLabel id="activity-select-label">Стать</InputLabel>
                                <Select
                                    labelId="activity-select-label"
                                    id="activity-select"
                                    value={watch('gender') || ''}
                                    label="Стать"
                                    {...register('gender', {required: "Вибір статі є обов'язковим!"})}
                                    onChange={(e) => setValue('gender', e.target.value)}
                                >
                                    <MenuItem value="Male">Чоловік</MenuItem>
                                    <MenuItem value="Female">Жінка</MenuItem>
                                </Select>
                                <Typography variant="body1" color="error">{errors?.gender?.message}</Typography>
                            </FormControl>
                            <FormControl fullWidth margin="normal" error={!!errors.dateOfBirth}>
                                <Controller
                                    name="dateOfBirth"
                                    control={control}
                                    rules={{ required: 'Вкажіть своє день народження!' }}
                                    render={({ field: { onChange, value } }) => (
                                        <DatePicker
                                            label="Дата народження"
                                            value={value || null}
                                            onChange={onChange}
                                            slotProps={{
                                                textField: {
                                                    error: !!errors.dateOfBirth,
                                                    helperText: errors?.dateOfBirth?.message
                                                }
                                            }}
                                        />
                                    )}
                                />
                            </FormControl>

                        </Grid>
                        <Grid size={8}>
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Поточний зріст"
                                {...register('height', {required: 'Введіть свій поточний зріст!'})}
                                error={!!errors.height}
                                helperText={errors?.height?.message as string}
                            />
                            <TextField
                                margin="normal"
                                fullWidth
                                label="Поточна вага"
                                {...register('weight', {required: 'Введіть свою поточну вагу!'})}
                                error={!!errors.weight}
                                helperText={errors?.weight?.message as string}
                            />
                            <FormControl fullWidth margin="normal" error={!!errors.goal}>
                                <InputLabel id="goal-select-label">Ціль</InputLabel>
                                <Select
                                    labelId="goal-select-label"
                                    id="goal-select"
                                    value={watch('goal') || ''}
                                    label="Ціль"
                                    {...register('goal', {required: "Встановлення цілі є обов'язковим"})}
                                    onChange={(e) => setValue('goal', e.target.value)}
                                >
                                    {goals.map(goal => (
                                        <MenuItem key={goal.id} value={goal.name}>{goal.name}</MenuItem>
                                    ))}
                                </Select>
                                <Typography variant="body2" color="error">{errors?.goal?.message}</Typography>
                            </FormControl>
                            <FormControl fullWidth margin="normal" error={!!errors.activity}>
                                <InputLabel id="activity-select-label">Рівень активності</InputLabel>
                                <Select
                                    labelId="activity-select-label"
                                    id="activity-select"
                                    value={watch('activity') || ''}
                                    label="Рівень активності"
                                    {...register('activity', {required: "Встановлення рівня активності є обов'язковим"})}
                                    onChange={(e) => setValue('activity', e.target.value)}
                                >
                                    {activities.map(activity => (
                                        <MenuItem key={activity.id} value={activity.name}>{activity.name}</MenuItem>
                                    ))}
                                </Select>
                                <Typography variant="body2" color="error">{errors?.activity?.message}</Typography>
                            </FormControl>
                        </Grid>
                    </Grid>
                    <Button loading={isSubmitting} disabled={!isValid} type="submit" fullWidth variant="contained" sx={{mt: 3, mb: 2}}>
                        Зареєструватись
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
    )
}
