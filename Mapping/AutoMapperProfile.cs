using AutoMapper;
using Prolance.Domain.Entities;
using Prolance.Application.DTOs;

namespace Prolance.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDTO>()
                .ReverseMap();
            CreateMap<Currency, CurrencyDto>()
                .ReverseMap();
            CreateMap<Project, ProjectDto>()
                .ReverseMap();
        }
    }
}
