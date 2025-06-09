using MediatR;

namespace Iduca.Application.Features.Courses.Delete;

public sealed record DeleteCourseRequest(
    Guid Id
) : IRequest<DeleteCourseResponse>;