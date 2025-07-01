using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Modules.Update;

public class UpdateModuleMapper : Profile
{
    public UpdateModuleMapper()
    {
        CreateMap<Module, UpdateModuleResponse>();
    }
}
