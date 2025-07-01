using Iduca.Domain.Models;
using AutoMapper;
using Iduca.Application.Features.User.Get;

namespace Iduca.Application.Features.User.Get;

public class GetUserMapper : Profile
{
    public GetUserMapper()
    {
        CreateMap<Domain.Models.User, GetUserResponse>()
            .ForMember(dest => dest.ResponsibleName, opt => opt.MapFrom(src => src.Responsible != null ? src.Responsible.Name : null))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests.Select(i => new CategoryInfo(i.Id, i.Name))));
    }
}
