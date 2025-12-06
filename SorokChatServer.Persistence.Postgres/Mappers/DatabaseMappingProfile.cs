using AutoMapper;
using SorokChatServer.Domain.Models;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres.Mappers;

public class DatabaseMappingProfile : Profile
{
    public DatabaseMappingProfile()
    {
        CreateMap<User, UserEntity>().ReverseMap();
    }
}