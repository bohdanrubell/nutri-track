using AutoMapper;
using NutriTrack.DTO;
using NutriTrack.Entities;

namespace NutriTrack;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UpdateProductNutritionRequest, ProductNutrition>();
        CreateMap<CreateProductNutritionRequest, ProductNutrition>();
    }
}