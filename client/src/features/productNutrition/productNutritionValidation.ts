import * as yup from 'yup'


export const validationSchema = yup.object({
    name: yup.string().required("Назва продукту обов'язкове!"),
    categoryName: yup.string().required("Вибір категорії обов'язково!"),
    caloriesPer100Grams: yup
        .number()
        .typeError("Калорії є обов'язкові")
        .required("Калорії є обов'язкові")
        .min(0, "Калорії не можуть бути менше 0")
        .max(1000, "Калорії не можуть перевищувати 1000"),
    proteinPer100Grams: yup
        .number()
        .typeError("Білки є обов'язкові")
        .required("Білки є обов'язкові")
        .min(0, "Білки не можуть бути менше 0")
        .max(1000, "Білки не можуть перевищувати 1000"),
    fatPer100Grams: yup
        .number()
        .typeError("Жири є обов'язкові")
        .required("Жири є обов'язкові")
        .min(0, "Жири не можуть бути менше 0")
        .max(1000, "Жири не можуть перевищувати 1000"),
    carbohydratesPer100Grams: yup
        .number()
        .typeError("Вуглеводи є обов'язкові")
        .required("Вуглеводи є обов'язкові")
        .min(0, "Вуглеводи не можуть бути менше 0")
        .max(1000, "Вуглеводи не можуть перевищувати 1000")
});