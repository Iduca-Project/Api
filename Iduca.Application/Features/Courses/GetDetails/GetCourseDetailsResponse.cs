namespace Iduca.Application.Features.Courses.GetDetails;

public sealed record GetCourseDetailsResponse(
    Guid Id,
    string Title,
    string Image,
    string Description,
    double Rating,
    int Participants,
    int Progress,
    int Difficulty,
    string Duration,
    string Category,
    bool HaveExam,
    List<ModuleDetails> Modules
);

public sealed record ModuleDetails(
    Guid Id,
    string Title,
    string Description,
    int? Index,
    List<ContentDetails> Content
);

public sealed record ContentDetails(
    Guid Id,
    int Type,
    string Title,
    bool Completed
);
