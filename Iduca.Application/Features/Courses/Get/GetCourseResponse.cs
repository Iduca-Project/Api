namespace Iduca.Application.Features.Courses.Get;

public sealed record GetCourseResponse (
    string Name,
    string Description,
    int Difficulty,
    string Image,
    int TotalHours,
    int Students,
    List<CategoryProps> Categories
);

public sealed record CategoryProps(
    string Name
);