namespace Iduca.Application.Features.Courses.Enroll;

public sealed record EnrollCourseResponse(
    Guid UserId,
    Guid CourseId,
    DateTime EnrolledAt,
    string Message
);
