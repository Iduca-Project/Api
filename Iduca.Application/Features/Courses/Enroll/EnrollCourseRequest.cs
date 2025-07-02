using MediatR;

namespace Iduca.Application.Features.Courses.Enroll;

public sealed record EnrollCourseRequest(
    Guid CourseId,
    Guid UserId
) : IRequest<EnrollCourseResponse>;
