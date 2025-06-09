
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.GetByModuleId;

public sealed record GetByModuleIdLessonResponse(
    List<Lesson> Lessons
);
