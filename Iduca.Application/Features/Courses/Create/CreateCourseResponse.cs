namespace Iduca.Application.Features.Companies.Create;

public sealed record CreateCourseResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);