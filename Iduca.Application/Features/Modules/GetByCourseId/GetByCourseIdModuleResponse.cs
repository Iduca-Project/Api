using Iduca.Domain.Models;

namespace Iduca.Application.Features.Modules.Create;

public sealed record GetByCourseIdModuleResponse(
    List<Module> Modules
);