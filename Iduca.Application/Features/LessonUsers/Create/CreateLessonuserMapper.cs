
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.LessonUsers.Create;

public class CreateLessonuserMapper : Profile
{
    public CreateLessonuserMapper()
    {
        CreateMap<CreateLessonuserRequest, Lessonuser>();
        CreateMap<Lessonuser, CreateLessonuserResponse>();
    }
}
