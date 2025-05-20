using Iduca.Domain.Models;

namespace Iduca.Application.Features.Courses.GetByQuery;

public sealed record GetCoursesResponse(
    List<Company> Companies
);