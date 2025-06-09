using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Companies.Get;

public class GetCompanyMapper : Profile
{
    public GetCompanyMapper()
    {
        CreateMap<GetCompanyRequest, Company>();
        CreateMap<Company, GetCompanyResponse>();
    }
}