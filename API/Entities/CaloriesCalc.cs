using NutriTrack.Entity.Enums;

namespace NutriTrack.Entities;

public class CaloriesCalc(Gender gender, int age, int height, int weight, ActivityLevel activityLevel, GoalType goalType)
{
    private Gender UserGender { get; set; } = gender;
    private int Age { get; set; } = age;
    private int Height { get; set; } = height;
    private int Weight { get; set; } = weight;
    private ActivityLevel Activity { get; set; } = activityLevel;
    private GoalType Goal { get; set; } = goalType;

    private decimal CalculateBmr()
    {
        if (UserGender == Gender.Male)
        {
            return 10m * Weight + 6.25m * Height - 5m * Age + 5m;
        }
        else
        {
            return 10m * Weight + 6.25m * Height - 5m * Age - 161m;
        }
    }

    public int CalculateDailyCalories()
    {
        var activityMultiplier = Activity.Ratio / 1000m;
        var goalModifier = Goal.Percent / 100m;
        var bmr = CalculateBmr();

        return (int)(bmr * activityMultiplier * goalModifier);
    }

    public decimal CalculateDailyProtein()
    {
        var calories = CalculateDailyCalories();
        return Math.Round(calories * 0.20m / 4m, 1);
    }

    public decimal CalculateDailyFat()
    {
        var calories = CalculateDailyCalories();
        return Math.Round(calories * 0.25m / 9m, 1);
    }

    public decimal CalculateDailyCarbohydrates()
    {
        var calories = CalculateDailyCalories();
        return Math.Round(calories * 0.55m / 4m, 1);
    }
}