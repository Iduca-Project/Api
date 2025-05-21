using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Companies.Get;

public class GetCourseByQueryCompanyMapper : Profile
{
    public GetCourseByQueryCompanyMapper()
    {
        CreateMap<Course, GetCompanyResponse>();
    }
}