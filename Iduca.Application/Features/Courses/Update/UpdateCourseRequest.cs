using Iduca.Domain.Common.Enums;
using MediatR;

namespace Iduca.Application.Features.Courses.Update;

public sealed record UpdateCourseRequest (
    Guid Id,
    string Name,
    string Description,
    CourseDifficulty Difficulty,
    string Image,
    int TotalHours,
    List<Guid> Categories
) : IRequest<UpdateCourseResponse>;
