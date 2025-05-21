using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.GetByQuery;

public sealed record GetCoursesRequest(
    string? Name,
    CourseDifficulty? Difficulty,
    Guid? Category
) : IRequest<GetCoursesResponse>;
