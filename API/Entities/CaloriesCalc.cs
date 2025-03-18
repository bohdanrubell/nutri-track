using NutriTrack.Entity.Enums;

namespace NutriTrack.Entities;

public class CaloriesCalc(Gender gender, int age, int height, double weight, ActivityLevel activityLevel, GoalType goalType)
{
    private Gender UserGender { get; set; } = gender;
    private int Age { get; set; } = age;
    private int Height { get; set; } = height;
    private double Weight { get; set; } = weight;
    private ActivityLevel Activity { get; set; } = activityLevel;
    private GoalType Goal { get; set; } = goalType;

    private double CalculateBmr()
    {
        if (UserGender == Gender.Male)
        {
            return 10 * Weight + 6.25 * Height - 5 * Age + 5;
        }
        else
        {
            return 10 * Weight + 6.25 * Height - 5 * Age - 161;
        }
    }

    public int CalculateDailyCalories()
    {
        var activityMultiplier = (double)Activity.Ratio / 1000;
        var interestDependFromGoal = (double)Goal.Percent / 100;
        var bmr = CalculateBmr();
        
        return (int)(bmr * activityMultiplier * interestDependFromGoal);
    }

    public double CalculateDailyProtein()
    {
        var calories = CalculateDailyCalories();
        
        return Math.Round(calories * 0.20 / 4, 2);
    }
    
    public double CalculateDailyFat()
    {
        var calories = CalculateDailyCalories();
        
        return Math.Round(calories * 0.25 / 9, 2);
    }
    
    public double CalculateDailyCarbohydrates()
    {
        var calories = CalculateDailyCalories();
        
        return Math.Round(calories * 0.55 / 4, 2);
    }
}