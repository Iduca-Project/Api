using Iduca.Domain.Models;

namespace Iduca.Application.Features.Courses.Update;

public sealed record UpdateCourseResponse(
    string Name,
    Guid Id,
    DateTime? UpdatedAt,
    List<Category> Categories,
    List<Module?> Modules,
    int Students
);