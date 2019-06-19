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
            CreateMap<NewStationDto, Station>()
                .ForPath(dest => dest.Address.City,
                    opt => { opt.MapFrom(src => src.City); })
                .ForPath(dest => dest.Address.Street,
                    opt => { opt.MapFrom(src => src.Street); })
                .ForPath(dest => dest.Address.Number,
                    opt => { opt.MapFrom(src => src.Number); })
                .ForPath(dest => dest.Location.X,
                    opt => { opt.MapFrom(src => src.X); })
                .ForPath(dest => dest.Location.Y,
                    opt => { opt.MapFrom(src => src.Y); });
            CreateMap<NewLineDto, Line>()
                .ForMember(dest => dest.Name,
                    opt => { opt.MapFrom(src => src.Name); })
                .ForMember(dest => dest.LineNumber,
                    opt => { opt.MapFrom(src => src.LineNumber); })
                .ForMember(dest => dest.Buses,
                    opt => { opt.MapFrom(src => src.Buses); })
                //.ForMember(dest => dest.Timetable,
                //    opt => { opt.Ignore(); })
                .ForMember(dest => dest.TimetableId,
                    opt => { opt.Ignore(); });
            CreateMap<NewLineDto, Line>()
                .ForMember(dest => dest.Buses,
                    opt => { opt.MapFrom(src => src.Buses); })
                .ForMember(dest => dest.Name,
                    opt => { opt.MapFrom(src => src.Name); })
                .ForMember(dest => dest.LineNumber,
                    opt => { opt.MapFrom(src => src.LineNumber); })
                .ForMember(dest => dest.Stations,
                    opt => opt.Ignore());
            CreateMap<NewPricelistDto, PricelistItem>()
                .ForPath(dest => dest.Item.Type,
                    opt => { opt.Ignore(); })
                .ForPath(dest => dest.Pricelist.Active,
                    opt => { opt.MapFrom(src => src.Active); })
                .ForPath(dest => dest.Pricelist.From,
                    opt => { opt.MapFrom(src => src.From); })
                .ForPath(dest => dest.Pricelist.To,
                    opt => { opt.MapFrom(src => src.To); })
                .ForMember(dest => dest.Price,
                    opt => { opt.Ignore(); });
            CreateMap<PricelistItem, PricelistItem>()
                .ForPath(dest => dest.Item.Type,
                    opt => { opt.MapFrom(src => src.Item.Type); })
                .ForPath(dest => dest.Pricelist.Active,
                    opt => { opt.MapFrom(src => src.Pricelist.Active); })
                .ForPath(dest => dest.Pricelist.From,
                    opt => { opt.MapFrom(src => src.Pricelist.From); })
                .ForPath(dest => dest.Pricelist.To,
                    opt => { opt.MapFrom(src => src.Pricelist.To); })
                .ForMember(dest => dest.Id,
                    opt => { opt.Ignore(); });
            CreateMap<TimeTable, TimeTable>()
                .ForMember(dest => dest.Day,
                    opt => { opt.MapFrom(src => src.Day); })
                .ForMember(dest => dest.Departures,
                    opt => { opt.MapFrom(src => src.Departures); })
                .ForMember(dest => dest.Id,
                    opt => { opt.Ignore(); })
                .ForMember(dest => dest.Type,
                    opt => { opt.MapFrom(src => src.Type); })
                .ForMember(dest => dest.Line,
                    opt => { opt.MapFrom(src => src.Line); })
                .ForMember(dest => dest.LineId,
                        opt => { opt.MapFrom(src => src.Line.Id); });
        }
    }
}
