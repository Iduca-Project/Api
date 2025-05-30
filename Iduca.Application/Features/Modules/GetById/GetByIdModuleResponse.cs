
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Module_.GetById;

public sealed record GetByIdModuleResponse(
    Guid Id,
    string Name,
    string Description,
    List<Exercise> Exercises
);
