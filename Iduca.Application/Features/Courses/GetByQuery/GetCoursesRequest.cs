using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.GetByQuery;

public sealed record GetCoursesRequest(
    string? Name,
    int? Difficulty,
    Guid? Category,
    int Page,
    int MaxItems
) : IRequest<GetCoursesResponse>;
