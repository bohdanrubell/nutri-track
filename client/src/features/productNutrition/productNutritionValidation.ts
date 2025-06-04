import * as yup from 'yup'


export const validationSchema = yup.object({
    name: yup
        .string()
        .required("Назва продукту обов'язкове!")
        .min(5, "Назва продукту має містити мінімум 5 символів")
        .max(50, "Назва продукту не може бути довшою за 50 символів"),
    categoryName: yup.string().required("Вибір категорії обов'язково!"),
    caloriesPer100Grams: yup
        .number()
        .typeError("Калорії є обов'язкові")
        .required("Калорії є обов'язкові")
        .min(10, "Калорії не можуть бути менше 10")
        .max(1000, "Калорії не можуть перевищувати 1000"),
    proteinPer100Grams: yup
        .number()
        .typeError("Білки є обов'язкові")
        .required("Білки є обов'язкові")
        .min(0, "Білки не можуть бути менше 0")
        .max(200, "Білки не можуть перевищувати 200"),
    fatPer100Grams: yup
        .number()
        .typeError("Жири є обов'язкові")
        .required("Жири є обов'язкові")
        .min(0, "Жири не можуть бути менше 0")
        .max(100, "Жири не можуть перевищувати 100"),
    carbohydratesPer100Grams: yup
        .number()
        .typeError("Вуглеводи є обов'язкові")
        .required("Вуглеводи є обов'язкові")
        .min(0, "Вуглеводи не можуть бути менше 0")
        .max(200, "Вуглеводи не можуть перевищувати 200")
});