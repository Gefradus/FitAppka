using AutoMapper;
using FitAppka.Models;

namespace FitAppka
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<CreateGoalsModel, Client>();
            // CreateMap<Client, CreateGoalsModel>();
            //CreateMap<CreateGoalsModel, Goals>();
            //CreateMap<Goals, Goals>().ForMember(g => g.ClientId, opt => opt.Ignore());


            CreateMap<Goals, CreateGoalsModel>()
                .ForMember(g => g.AutoDietaryGoals, opt => opt.Ignore())
                .ForMember(g => g.IncludeCaloriesBurned, opt => opt.Ignore());
        }
    }
}
