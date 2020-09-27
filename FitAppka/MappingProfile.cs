using AutoMapper;
using FitAppka.Models;
using FitAppka.Service.ServiceImpl;
using FitAppka.Strategy.ChartTypeStrategyImpl;
using FitAppka.Strategy.StrategyImpl.FindDayStrategyImpl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitAppka
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Goals, Goals>()
                .ForMember(g => g.GoalsId, opt => opt.Ignore())
                .ForMember(g => g.DayId, opt => opt.Ignore())
                .ForMember(g => g.Day, opt => opt.Ignore())
                .ForMember(g => g.DayNavigation, opt => opt.Ignore())
                .ForMember(g => g.Client, opt => opt.Ignore())
                .ForMember(g => g.ClientId, opt => opt.Ignore())
                .ForMember(g => g.ClientNavigation, opt => opt.Ignore());

            CreateMap<Client, Day>()
                .ForMember(d => d.WaterDrunk, opt => opt.MapFrom(water => 0))
                .ForMember(d => d.GoalsId, opt => opt.Ignore())
                .ForMember(d => d.GoalsNavigation, opt => opt.Ignore())
                .ForMember(d => d.Goals, opt => opt.Ignore()); 

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

            CreateMap<List<Product>, List<FindDayProductDTO>>();

            CreateMap<Task<List<Product>>, Task<List<ProductDTO>>>();

            CreateMap<ProgressMonitoringServiceImpl, CaloriesConsumedChartStrategy>();
            CreateMap<ProgressMonitoringServiceImpl, CaloriesBurnedChartStrategy>();
            CreateMap<ProgressMonitoringServiceImpl, CardioTrainingTimeChartStrategy>();
            CreateMap<ProgressMonitoringServiceImpl, EstimatedBodyFatChartStrategy>();
            CreateMap<ProgressMonitoringServiceImpl, WaistCircumferenceMeasurementChartStrategy>();
            CreateMap<ProgressMonitoringServiceImpl, WaterConsumptionChartStrategy>();
            CreateMap<ProgressMonitoringServiceImpl, WeightMeasurementChartStrategy>();

            CreateMap<FindDayServiceImpl, FindDayByCaloriesConsumedStrategy>();
            CreateMap<FindDayServiceImpl, FindDayByProductConsumedStrategy>();
            CreateMap<FindDayServiceImpl, FindDayByWaterDrunkStrategy>();

        }
    }
}
