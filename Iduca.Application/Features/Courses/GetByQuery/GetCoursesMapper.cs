using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Courses.GetByQuery;

public class GetCoursesMapper : Profile
{
    public GetCoursesMapper()
    {
        CreateMap<Course, GetCoursesResponse>();
        CreateMap<Category, CategoryProps>();
        CreateMap<Course, GetCourseProps>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));
    }
}