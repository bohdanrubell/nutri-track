namespace NutriTrack.RequestHelpers;

public class ProductNutritionParams : PaginationParams
{
    public string? OrderBy { get; set; }
    public string? SearchTerm { get; set; }
    public string? ProductNutritionCategory { get; set; }
}