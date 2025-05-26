using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Courses.Get;

public class GetCourseMapper : Profile
{
    public GetCourseMapper()
    {
        CreateMap<Course, GetCourseResponse>();
        CreateMap<Category, CategoryProps>();
        CreateMap<Course, GetCourseResponse>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));
    }
}