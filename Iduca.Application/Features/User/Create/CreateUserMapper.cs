using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.User.Create;

public class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<Domain.Models.User, CreateUserResponse>();
    }
}
