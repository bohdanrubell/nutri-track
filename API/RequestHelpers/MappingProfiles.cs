using AutoMapper;
using NutriTrack.DTO.ProductNutrition;
using NutriTrack.Entities;

namespace NutriTrack.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UpdateProductNutritionRequest, ProductNutrition>();
        CreateMap<CreateProductNutritionRequest, ProductNutrition>();
    }
}