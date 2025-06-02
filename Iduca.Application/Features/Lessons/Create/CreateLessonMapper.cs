
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.Create;

public class CreateLessonMapper : Profile
{
    public CreateLessonMapper()
    {
        CreateMap<CreateLessonRequest, Lesson>();
        CreateMap<Lesson, CreateLessonResponse>();
    }
}
