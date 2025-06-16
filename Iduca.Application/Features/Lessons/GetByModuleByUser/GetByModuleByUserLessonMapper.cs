
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.GetByModuleByUser;

public class GetByModuleByUserLessonMapper : Profile
{
    public GetByModuleByUserLessonMapper()
    {
        CreateMap<GetByModuleByUserLessonRequest, Lesson>();
        CreateMap<Lesson, GetByModuleByUserLessonResponse>();
    }
}
