using Iduca.Domain.Models;
using AutoMapper;
using Iduca.Application.Features.Companies.Create;

namespace Iduca.Application.Features.Courses.Create;

public class CreateCourseMapper : Profile
{
    public CreateCourseMapper()
    {
        CreateMap<CreateCourseRequest, Course>();
        CreateMap<Course, CreateCourseResponse>();
    }
}