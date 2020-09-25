using AutoMapper;
using FitAppka.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            CreateMap<ProductDTO, Product>()
                .ForMember(p => p.PhotoPath, opt => opt.Ignore())
                .ForMember(p => p.ClientId, opt => opt.Ignore())
                .ForMember(p => p.Meal, opt => opt.Ignore())
                .ForMember(p => p.Client, opt => opt.Ignore())
                .ForMember(p => p.VisibleToAll, opt => opt.Ignore());

            CreateMap<Product, Product>()
                .ForMember(p => p.PhotoPath, opt => opt.MapFrom(p => p.PhotoPath))
                .ForMember(p => p.ClientId, opt => opt.MapFrom(p => p.ClientId))
                .ForMember(p => p.Meal, opt => opt.MapFrom(p => p.Meal))
                .ForMember(p => p.Client, opt => opt.MapFrom(p => p.Client))
                .ForMember(p => p.VisibleToAll, opt => opt.MapFrom(p => p.VisibleToAll))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Task<List<Product>>, Task<List<ProductDTO>>>();

            CreateMap<WeightMeasurement, WeightMeasurementDTO>();
        }
    }
}
