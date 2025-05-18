using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Companies.Create;

public class CreateCompanyMapper : Profile
{
    public CreateCompanyMapper()
    {
        CreateMap<CreateCompanyRequest, Company>();
        CreateMap<Company, CreateCompanyResponse>();
    }
}