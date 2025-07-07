
using MediatR;

namespace Iduca.Application.Features.Courses.GetCourseOfUser;

public sealed record GetCourseOfUserCourseRequest(
    Guid UserId
) : IRequest<GetCourseOfUserCourseResponse>;
