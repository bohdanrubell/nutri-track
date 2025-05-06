import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { Box, Paper } from "@mui/material";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { FieldValues, useForm } from "react-hook-form";
import { LoadingButton } from "@mui/lab";
import { toast } from "react-toastify";
import { useEffect, useState } from "react";
import { yupResolver } from "@hookform/resolvers/yup";
import { resetPasswordValidationSchema } from "../../validationSchemas/passwordResetSchema";
import apiClient from "../../app/axios/apiClient";

export default function ResetPassword() {
    const [searchParams] = useSearchParams();
    const email = searchParams.get('email') || '';
    const encodedToken = searchParams.get('token') || '';
    const navigate = useNavigate();
    const [resetSuccess, setResetSuccess] = useState(false);
    
    const { register, handleSubmit, formState: { isSubmitting, errors, isValid } } = useForm({
        mode: 'onTouched',
        resolver: yupResolver(resetPasswordValidationSchema)
    });

    useEffect(() => {
        console.log(email)
        console.log(encodedToken)
        if (!email || !encodedToken) {
            toast.error("Недійсне посилання для скидання паролю");
            navigate('/forgot-password');
        }
    }, [email, encodedToken, navigate]);

    async function submitForm(data: FieldValues) {
        try {
            console.log("Email:", email);
            console.log("Token (from URL):", encodedToken);
            console.log("New Password:", data.newPassword);
            
            await apiClient.Account.resetPassword(email, encodedToken, data.newPassword);
            setResetSuccess(true);
            toast.success("Пароль успішно змінено");
        } catch (error) {
            console.error("Error resetting password:", error);
            
            // More detailed error handling
            if (error.response && error.response.data) {
                console.error("Server error details:", error.response.data);
                
                // If the error contains specific validation errors
                if (error.response.data.errors) {
                    toast.error("Помилка валідації: " + JSON.stringify(error.response.data.errors));
                } else {
                    toast.error("Помилка сервера: " + JSON.stringify(error.response.data));
                }
            } else {
                toast.error("Помилка при зміні паролю. Можливо, посилання недійсне або термін його дії закінчився.");
            }
        }
    }

    return (
        <Container component={Paper} maxWidth="sm" sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 4 }}>
            <Typography component="h1" variant="h5">
                Зміна паролю
            </Typography>

            {resetSuccess ? (
                <Box sx={{ mt: 3, textAlign: 'center' }}>
                    <Typography variant="body1" gutterBottom>
                        Ваш пароль успішно змінено.
                    </Typography>
                    <Typography variant="body2" sx={{ mt: 2 }}>
                        <Link to="/login">Перейти на сторінку входу</Link>
                    </Typography>
                </Box>
            ) : (
                <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1, width: '100%' }}>
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Email"
                        value={email}
                        disabled
                    />
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Новий пароль"
                        type="password"
                        {...register('newPassword')}
                        error={!!errors.newPassword}
                        helperText={errors?.newPassword?.message as string}
                    />
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Підтвердіть новий пароль"
                        type="password"
                        {...register('confirmPassword')}
                        error={!!errors.confirmPassword}
                        helperText={errors?.confirmPassword?.message as string}
                    />
                    <LoadingButton 
                        loading={isSubmitting}
                        disabled={!isValid}
                        type="submit"
                        fullWidth
                        variant="contained" 
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Змінити пароль
                    </LoadingButton>
                </Box>
            )}
        </Container>
    );
} 