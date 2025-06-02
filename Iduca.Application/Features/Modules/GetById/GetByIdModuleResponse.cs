
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Modules.GetById;

public sealed record GetByIdModuleResponse(
    Guid Id,
    string Name,
    string Description,
    Guid CourseId,
    List<Exercise> Exercises,
    List<Lesson> Lessons
);
