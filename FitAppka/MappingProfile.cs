using AutoMapper;
using FitAppka.Models;

namespace FitAppka
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Goals, GoalsDTO>()
                .ForMember(g => g.AutoDietaryGoals, opt => opt.Ignore())
                .ForMember(g => g.IncludeCaloriesBurned, opt => opt.Ignore());

            CreateMap<Product, ProductDTO>()
                .ForMember(p => p.Photo, opt => opt.Ignore());
        }
    }
}
