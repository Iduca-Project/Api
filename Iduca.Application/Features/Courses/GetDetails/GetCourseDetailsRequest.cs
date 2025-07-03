using MediatR;

namespace Iduca.Application.Features.Courses.GetDetails;

public sealed record GetCourseDetailsRequest(
    Guid CourseId,
    Guid? UserId = null
) : IRequest<GetCourseDetailsResponse>;
