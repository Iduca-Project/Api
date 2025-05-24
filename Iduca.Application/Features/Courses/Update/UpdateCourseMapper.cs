using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Courses.Update;

public class UpdateCourseMapper : Profile
{
    public UpdateCourseMapper()
    {
        CreateMap<UpdateCourseRequest, Course>();
        CreateMap<Course, UpdateCourseResponse>();
    }
}