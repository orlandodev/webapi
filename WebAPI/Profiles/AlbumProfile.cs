using AutoMapper;
using WebAPI.DataAccess.Models;
using WebAPI.Services.DTOs;

namespace WebAPI.Profiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<AlbumCreateDTO, Album>();
            CreateMap<AlbumUpdateDTO, Album>();
        }
    }
}
