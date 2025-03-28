export interface DailyRecord{
    dailyNutritions: DailyNutritions;
    productRecords: ProductRecord[];
}

interface DailyNutritions{
    dailyCalories: number;
    dailyProtein: number;
    dailyFat: number;
    dailyCarbohydrates: number;
}

interface ProductRecord{
    id: number;
    name: string;
    grams: number;
    calories: number;
    protein: number;
    fat: number;
    carbohydrates: number;
}

export default interface ProductRecordForm{
    productRecordId: number;
    consumedGrams: number;
}