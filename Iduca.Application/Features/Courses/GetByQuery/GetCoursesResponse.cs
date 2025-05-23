using Iduca.Domain.Models;

namespace Iduca.Application.Features.Courses.GetByQuery;

public sealed record GetCoursesResponse(
    List<GetCourseProps> Courses
);

public sealed record GetCourseProps (
    string Name,
    string Description,
    int Difficulty,
    string Image,
    int TotalHours,
    List<CategoryProps> Categories
);

public sealed record CategoryProps(
    string Name
);