import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { Box, Paper } from "@mui/material";
import { Link } from "react-router-dom";
import { FieldValues, useForm } from "react-hook-form";
import { LoadingButton } from "@mui/lab";
import { toast } from "react-toastify";
import { useState } from "react";
import { yupResolver } from "@hookform/resolvers/yup";
import { forgotPasswordValidationSchema } from "../../validationSchemas/passwordResetSchema";
import apiClient from "../../app/axios/apiClient";

export default function ForgotPassword() {
    const [emailSent, setEmailSent] = useState(false);
    const { register, handleSubmit, formState: { isSubmitting, errors, isValid } } = useForm({
        mode: 'onTouched',
        resolver: yupResolver(forgotPasswordValidationSchema)
    });

    async function submitForm(data: FieldValues) {
        try {
            await apiClient.Account.forgotPassword(data.email);
            setEmailSent(true);
            toast.success("Якщо вказана адреса існує, на неї буде надіслано інструкції з відновлення паролю");
        } catch (error) {
            console.error("Error sending reset email:", error);
            toast.error("Помилка при відправленні email. Спробуйте ще раз.");
        }
    }

    return (
        <Container component={Paper} maxWidth="sm" sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 4 }}>
            <Typography component="h1" variant="h5">
                Відновлення паролю
            </Typography>
            {emailSent ? (
                <Box sx={{ mt: 3, textAlign: 'center' }}>
                    <Typography variant="body1" gutterBottom>
                        Якщо вказана адреса електронної пошти зареєстрована в системі, 
                        ми надіслали на неї інструкції з відновлення паролю.
                    </Typography>
                    <Typography variant="body2" sx={{ mt: 2 }}>
                        <Link to="/login">Повернутися на сторінку входу</Link>
                    </Typography>
                </Box>
            ) : (
                <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1, width: '100%' }}>
                    <Typography variant="body2" sx={{ mb: 2 }}>
                        Введіть адресу електронної пошти, і ми надішлемо вам посилання для скидання паролю.
                    </Typography>
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Email"
                        autoFocus
                        type="email"
                        {...register('email')}
                        error={!!errors.email}
                        helperText={errors?.email?.message as string}
                    />
                    <LoadingButton 
                        loading={isSubmitting}
                        disabled={!isValid}
                        type="submit"
                        fullWidth
                        variant="contained" 
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Надіслати інструкції
                    </LoadingButton>
                    <Box sx={{ textAlign: 'center', mt: 1 }}>
                        <Link to="/login">Повернутися на сторінку входу</Link>
                    </Box>
                </Box>
            )}
        </Container>
    );
} 