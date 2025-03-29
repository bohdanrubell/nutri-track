export interface ProductNutrition {
    id: number,
    name: string,
    calories: number,
    protein: number,
    fat: number,
    carbohydrates: number
}

export interface ProductNutritionParams {
    orderBy: string;
    searchTerm?: string;
    categories: ProductNutritionCategory[];
    pageNumber: number;
    pageSize: number;
}

export interface ProductNutritionCategory{
    name: string
}

export default interface ProductRecordNew{
    productNutritionId: number;
    consumedGrams: number;
}