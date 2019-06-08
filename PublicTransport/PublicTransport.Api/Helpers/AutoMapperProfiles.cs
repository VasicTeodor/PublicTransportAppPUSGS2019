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
                .ForPath(dest => dest.Address.City,
                opt => { opt.MapFrom(src => src.City); })
                .ForPath(dest => dest.Address.Number,
                opt => { opt.MapFrom(src => src.Number); })
                .ForPath(dest => dest.Address.Street,
                opt => { opt.MapFrom(src => src.Street); });
            CreateMap<User, UserForDisplayDto>()
                .ForMember(dest => dest.City,
                    opt => { opt.MapFrom(src => src.Address.City); })
                .ForMember(dest => dest.Street,
                    opt => { opt.MapFrom(src => src.Address.Street); })
                .ForMember(dest => dest.Number,
                    opt => { opt.MapFrom(src => src.Address.Number); });
            CreateMap<UserForUpdateDto, User>()
                .ForPath(dest => dest.Address.City,
                    opt => { opt.MapFrom(src => src.City); })
                .ForPath(dest => dest.Address.Number,
                    opt => { opt.MapFrom(src => src.Number); })
                .ForPath(dest => dest.Address.Street,
                    opt => { opt.MapFrom(src => src.Street); });
            CreateMap<PhotoUploadDto, User>()
                .ForMember(dest => dest.DocumentUrl,
                    opt => { opt.MapFrom(src => src.Url); })
                .ForMember(dest => dest.PublicId,
                    opt => { opt.MapFrom(src => src.PublicId); });
        }
    }
}
