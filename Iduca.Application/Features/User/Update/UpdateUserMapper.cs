using Iduca.Domain.Models;
using AutoMapper;
using Iduca.Application.Features.User.Update;

namespace Iduca.Application.Features.User.Update;

public class UpdateUserMapper : Profile
{
    public UpdateUserMapper()
    {
        CreateMap<Domain.Models.User, UpdateUserResponse>();
    }
}
