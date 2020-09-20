using AutoMapper;
using FitAppka.Models;

namespace FitAppka
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateGoalsModel, Client>();
            CreateMap<Client, CreateGoalsModel>();
            CreateMap<Goals, Goals>().ForMember(g => g.ClientId, opt => opt.Ignore());
        }
    }
}
