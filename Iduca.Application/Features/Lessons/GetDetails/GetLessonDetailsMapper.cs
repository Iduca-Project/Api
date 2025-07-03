using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.GetDetails;

public class GetLessonDetailsMapper : Profile
{
    public GetLessonDetailsMapper()
    {
        CreateMap<Lesson, GetLessonDetailsResponse>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Module.CourseId))
            .ForMember(dest => dest.Completed, opt => opt.Ignore())
            .ForMember(dest => dest.NextLesson, opt => opt.Ignore())
            .ForMember(dest => dest.Module, opt => opt.Ignore());
    }
}
