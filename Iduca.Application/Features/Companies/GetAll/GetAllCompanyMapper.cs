using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Companies.GetAll;

public class GetAllCompanyMapper : Profile
{
    public GetAllCompanyMapper()
    {
        CreateMap<GetAllCompanyRequest, Company>();
        CreateMap<Company, GetAllCompanyResponse>();
    }
}