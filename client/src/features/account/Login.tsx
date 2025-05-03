import TextField from '@mui/material/TextField';
import Grid from '@mui/material/Grid2';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import {Box, Paper} from "@mui/material";
import {Link, useLocation, useNavigate} from "react-router-dom";
import {FieldValues, useForm} from "react-hook-form";
import {LoadingButton} from "@mui/lab";
import {useAppDispatch} from "../../app/store/store.ts";
import {signInUser} from "./accountSlice.tsx";

export default function Login() {

    const navigate = useNavigate();
    const location = useLocation();
    const dispatch = useAppDispatch();
    const {register,
        handleSubmit,
        formState: {isSubmitting, errors,  isValid}} =  useForm({
        mode: 'onTouched'
    });

    async function submitForm(data: FieldValues) {
        await dispatch(signInUser(data));
        navigate(location.state?.from || '/productNutrition');
    }

    return (
        <Container component={Paper} maxWidth="sm" sx={{display: 'flex', flexDirection: 'column', alignItems: 'center', p: 4}}>
            <Typography component="h1" variant="h5">
                Авторизація користувача
            </Typography>
            <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1 }}>
                <TextField
                    margin="normal"
                    fullWidth
                    label="Ім'я користувача"
                    autoFocus
                    {...register('username', {required: "Ім'я користувача є обов'язковим"})}
                    error = {!!errors.username}
                    helperText={errors?.username?.message as string}
                />
                <TextField
                    margin="normal"
                    fullWidth
                    label="Пароль"
                    type="password"
                    autoComplete="current-password"
                    {...register('password', {required: "Пароль користувача є обов'язковим"})}
                    error = {!!errors.password}
                    helperText={errors?.password?.message as string}
                />
                <LoadingButton loading={isSubmitting}
                               disabled={!isValid}
                               type="submit"
                               fullWidth
                               variant="contained" sx={{ mt: 3, mb: 2 }}
                >
                    Авторизуватись
                </LoadingButton>
                <Grid container
                      direction="row"
                      sx={{
                          justifyContent: "center",
                          alignItems: "center",
                      }}
                >
                    <Grid>
                        <Link to='/register'>
                            {"Немає аккаунту? Зареєструйтесь!"}
                        </Link>
                    </Grid>
                </Grid>
            </Box>
        </Container>
    )
}