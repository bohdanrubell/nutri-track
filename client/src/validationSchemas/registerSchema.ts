import { z } from 'zod';
import dayjs from "dayjs";

const today = dayjs();

export const registerSchema = z.object({
    username: z.string({
        required_error: "Ім'я є обов'язковим"
    })
        .nonempty("Ім'я є обов'язковим")
        .min(2, "Ім'я має містити мінімум 2 символи"),

    password: z.string({
        required_error: "Пароль є обов'язковим"
    })
        .min(6, "Пароль має містити мінімум 6 символів")
        .max(10, "Пароль має бути не довшим за 10 символів")
        .regex(
            /(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}[\]:;<>,.?/~`])/,
            "Пароль має містити цифру, велику та малу літери та спецсимвол"
        ),

    email: z.string({
        required_error: "Пошта є обов'язковою"
    })
        .email("Некоректний формат Email")
        .nonempty("Email є обов'язковим"),

    gender: z.string({
        required_error: "Стать є обов'язковою"
    })
        .nonempty("Оберіть стать"),

    dateOfBirth: z.string()
        .nonempty("Дата народження є обов'язковою")
        .refine((val) => dayjs(val).isValid(), {
            message: "Некоректна дата",
        })
        .refine((val) => dayjs(val).isBefore(today), {
            message: "Дата народження не може бути в майбутньому",
        }),

    height: z.number({
        required_error: "Зріст є обов'язковим"
    })
        .min(50, "Зріст має бути більше 50 см")
        .max(250, "Зріст має бути менше 250 см"),

    weight: z.number({
        required_error: "Вага є обов'язковою"
    })
        .min(20, "Вага має бути більше 20 кг")
        .max(300, "Вага має бути менше 300 кг"),

    goal: z.string({
        required_error: "Оберіть ціль"
    })
        .nonempty("Оберіть ціль"),

    activity: z.string({
        required_error: "Оберіть рівень активності"
    })
        .nonempty("Оберіть рівень активності"),
});

export type RegisterFormData = z.infer<typeof registerSchema>;
