using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Companies.GetById;

public class GetByIdCompanyMapper : Profile
{
    public GetByIdCompanyMapper()
    {
        CreateMap<GetByIdCompanyRequest, Company>();
        CreateMap<Company, GetByIdCompanyResponse>();
    }
}