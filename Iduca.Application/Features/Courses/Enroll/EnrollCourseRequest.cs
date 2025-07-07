using MediatR;

namespace Iduca.Application.Features.Courses.Enroll;

public sealed record EnrollCourseRequest(
    string CourseName,
    string Identity
) : IRequest<EnrollCourseResponse>;

public sealed record EnrollCourseRequestId(
    Guid CourseId,
    Guid UserId
) : IRequest<EnrollCourseResponse>;
