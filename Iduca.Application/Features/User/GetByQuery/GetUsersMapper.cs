using Iduca.Domain.Models;
using AutoMapper;
using Iduca.Application.Features.User.GetByQuery;

namespace Iduca.Application.Features.User.GetByQuery;

public class GetUsersMapper : Profile
{
    public GetUsersMapper()
    {
        CreateMap<Domain.Models.User, GetUsersResponse>()
            .ForMember(dest => dest.ResponsibleName, opt => opt.MapFrom(src => src.Responsible != null ? src.Responsible.Name : null))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));
    }
}
