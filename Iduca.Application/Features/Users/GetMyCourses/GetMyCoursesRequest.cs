using MediatR;

namespace Iduca.Application.Features.Users.GetMyCourses;

public sealed record GetMyCoursesRequest(
    Guid UserId
) : IRequest<GetMyCoursesResponse>;
