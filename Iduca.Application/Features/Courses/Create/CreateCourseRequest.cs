using Iduca.Application.Features.Companies.Create;
using Iduca.Domain.Common.Enums;
using MediatR;

namespace Iduca.Application.Features.Courses.Create;

public sealed record CreateCourseRequest(
    string Title,
    string Image,
    List<Guid> Categories,
    string Description,
    CourseDifficulty Difficulty,
    int Duration,

    ExamProps? Exam,
    List<ModuleProps> Modules

) : IRequest<CreateCourseResponse>;



public record ModuleProps(
    string Name,
    string Description,
    int? Index,
    List<LessonProps>? Lessons,
    List<ExerciseProps>? Exercises
);


public record LessonProps(
    string Name,
    string Description,
    List<ContentProps> Contents
);
public record ContentProps(
    string Title,
    string? Description,
    string? Image
);

public record ExamProps(
    string Title,
    string Description,
    List<QuestionProps> Questions
);

public record ExerciseProps(
    string Title,
    string Description,
    DateTime Date,
    List<QuestionProps> Questions
);

public record QuestionProps(
    string Description,
    List<AlternativeProps> Alternatives
);

public record AlternativeProps(
    string Description,
    bool IsCorrect
);