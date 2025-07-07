
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Courses.GetCourseOfUser;

public sealed record GetCourseOfUserCourseResponse(
    List<Course> Courses
);
