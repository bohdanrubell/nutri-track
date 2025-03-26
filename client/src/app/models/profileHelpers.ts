export interface GoalType{
    id: number;
    name: string;
}

export interface ActivityLevel{
    id: number;
    name: string;
}

export interface UserCharacteristics {
    gender: string;
    dateOfBirth: string;
    age: number;
    height: number;
    currentGoalType: string;
    currentActivityLevel: string;
    dailyNutritions: DailyNutritions;
    weightRecords: WeightRecord[];
}

export interface WeightRecord{
    date: string;
    weight: number;
}

export interface DailyNutritions{
    dailyCalories: number;
    dailyProtein: number;
    dailyFat: number;
    dailyCarbohydrates: number;
}

export interface ProfileFormData{
    gender: string;
    height: number;
    currentGoalType: string;
    currentActivityLevel: string;
}