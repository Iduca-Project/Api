
namespace Iduca.Application.Features.Exercises.GetExerciseDetails;

public sealed record GetExerciseDetailsExerciseResponse(
    Guid Id,
    string Title,
    List<QuestionProps> Questions
);

public record QuestionProps
(
    Guid Id,
    string Description,
    List<AlternativeProps> Alternatives
);

public record AlternativeProps
(
    Guid Id,
    string Description,
    bool IsCorrect
);
