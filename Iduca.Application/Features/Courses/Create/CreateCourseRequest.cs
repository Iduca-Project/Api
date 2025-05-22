using Iduca.Application.Features.Companies.Create;
using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.Create;

public sealed record CreateCourseRequest(
    string Name,
    string Description,
    CourseDifficulty Difficulty,
    string Image,
    int TotalHours,
    List<Guid> Categories
) : IRequest<CreateCourseResponse>;