using AutoMapper;
using PublicTransport.Api.Dtos;
using PublicTransport.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicTransport.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>()
                .ForPath(dest => dest.Adress.City,
                opt => { opt.MapFrom(src => src.City); })
                .ForPath(dest => dest.Adress.Number,
                opt => { opt.MapFrom(src => src.Number); })
                .ForPath(dest => dest.Adress.Street,
                opt => { opt.MapFrom(src => src.Street); });
        }
    }
}
