using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.Get;

public sealed record GetCourseRequest(
    Guid Id
) : IRequest<GetCourseResponse>;
