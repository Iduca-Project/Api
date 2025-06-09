
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.GetByModuleId;

public class GetByModuleIdLessonMapper : Profile
{
    public GetByModuleIdLessonMapper()
    {
        CreateMap<GetByModuleIdLessonRequest, Lesson>();
        CreateMap<Lesson, GetByModuleIdLessonResponse>();
    }
}
