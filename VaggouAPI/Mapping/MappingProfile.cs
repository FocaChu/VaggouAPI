using AutoMapper;
using VaggouAPI.DTOs;
using VaggouAPI.Models;

namespace VaggouAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Source, Destination>();
            // Example: CreateMap<User, UserDto>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>()
            .ForMember(dest=> dest.User, opt => opt.Ignore());

        }
    }
}
