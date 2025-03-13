using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Mapper
{
    public class MapperClass : Profile
    {
        public MapperClass()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>()
                .ForMember(roleDto => roleDto.RoleName, expression =>
                {
                    expression.MapFrom(src => src.Name);
                })
                .ReverseMap();
            CreateMap<Genre, GenreDto>()
                .ForMember(dest => dest.MovieTypes, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Shows, opt => opt.Ignore())
                .ForMember(dest => dest.MovieTypes, opt => opt.MapFrom(src => src.MovieTypes))
                .ForMember(dest => dest.GenreNames, opt => opt.MapFrom(src => src.MovieTypes.Select(mt => mt.Genre.Name).ToList()))
                .ReverseMap();
            CreateMap<Movie, MovieDetailDto>();
            CreateMap<Cinema, CinemaDto>().ReverseMap();
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<TimeOnly, ShowtimeDetail>()
                .ForMember(dest => dest.ShowTime, opt => opt.MapFrom(src => src));
            CreateMap<Show, ShowDto>()
                .ForMember(dest => dest.ShowTimeDetails, opt => opt.MapFrom(src => src.ShowTimeDetails.Select(std => std.ShowTime).ToList()))
                .ReverseMap();
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}
