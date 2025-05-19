using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Companies.Update;

public class UpdateCompanyMapper : Profile
{
    public UpdateCompanyMapper()
    {
        CreateMap<UpdateCompanyRequest, Company>();
        CreateMap<Company, UpdateCompanyResponse>();
    }
}