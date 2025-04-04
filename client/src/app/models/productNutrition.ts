export interface ProductNutrition {
    id: number,
    name: string,
    caloriesPer100Grams: number,
    proteinPer100Grams: number,
    fatPer100Grams: number,
    carbohydratesPer100Grams: number,
    imageId?: string
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