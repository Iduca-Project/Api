
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Courses.GetCourseOfUser;

public class GetCourseOfUserCourseMapper : Profile
{
    public GetCourseOfUserCourseMapper()
    {
        CreateMap<GetCourseOfUserCourseRequest, Course>();
        CreateMap<Course, GetCourseOfUserCourseResponse>();
    }
}
