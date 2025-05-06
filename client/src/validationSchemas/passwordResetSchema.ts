import * as yup from 'yup';

export const forgotPasswordValidationSchema = yup.object({
    email: yup.string()
        .required("Email є обов'язковим")
        .email("Неправильний формат email"),
});

export const resetPasswordValidationSchema = yup.object({
    newPassword: yup.string()
        .required("Пароль є обов'язковим")
        .min(6, "Пароль повинен містити щонайменше 6 символів"),
    confirmPassword: yup.string()
        .required("Підтвердження паролю є обов'язковим")
        .oneOf([yup.ref('newPassword')], "Паролі повинні співпадати"),
}); 