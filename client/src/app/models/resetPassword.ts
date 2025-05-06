export interface ForgotPasswordFormData {
    email: string;
}

export interface ResetPasswordFormData {
    email: string;
    token: string;
    newPassword: string;
    confirmPassword: string;
} 