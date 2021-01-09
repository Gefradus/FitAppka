using AutoMapper;
using FitnessApp.Models;
using FitnessApp.Models.DTO.DietCreatorDTO;
using FitnessApp.Service.ServiceImpl;
using FitnessApp.Strategy.ChartTypeStrategyImpl;
using FitnessApp.Strategy.StrategyImpl.DayOfWeekDietStrategyImpl;
using FitnessApp.Strategy.StrategyImpl.FindDayStrategyImpl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp
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

            CreateMap<Client, SettingsDTO>();

            CreateMap<Client, RegisterDTO>();

            CreateMap<Goals, GoalsDTO>()
                .ForMember(g => g.AutoDietaryGoals, opt => opt.Ignore())
                .ForMember(g => g.IncludeCaloriesBurned, opt => opt.Ignore());

            CreateMap<GoalsDTO, Goals>();

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

            CreateMap<Diet, DietDTO>();
            CreateMap<Product, DietProductDTO>()
                .AfterMap((src, dest) => dest.Calories = (double)Math.Round((decimal)(dest.Grammage * src.Calories / 100), 1, MidpointRounding.AwayFromZero))
                .AfterMap((src, dest) => dest.Carbohydrates = (double)Math.Round((decimal)(dest.Grammage * src.Carbohydrates / 100), 1, MidpointRounding.AwayFromZero))
                .AfterMap((src, dest) => dest.Proteins = (double)Math.Round((decimal)(dest.Grammage * src.Proteins / 100), 1, MidpointRounding.AwayFromZero))
                .AfterMap((src, dest) => dest.Fats = (double)Math.Round((decimal)(dest.Grammage * src.Fats / 100), 1, MidpointRounding.AwayFromZero));

            CreateMap<DietProductDTO, DietProduct>();

            CreateMap<DietDTO, Diet>().ForMember(d => d.DietId, opt => opt.Ignore());

            CreateMap<Product, SearchProductDTO>();

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

            CreateMap<DietCreatorServiceImpl, MondayDietStrategy>();
            CreateMap<DietCreatorServiceImpl, TuesdayDietStrategy>();
            CreateMap<DietCreatorServiceImpl, WednesdayDietStrategy>();
            CreateMap<DietCreatorServiceImpl, ThursdayDietStrategy>();
            CreateMap<DietCreatorServiceImpl, FridayDietStrategy>();
            CreateMap<DietCreatorServiceImpl, SaturdayDietStrategy>();
            CreateMap<DietCreatorServiceImpl, SundayDietStrategy>();
        }
    }
}
