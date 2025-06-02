using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Modules.Create;

public class CreateModuleMapper : Profile
{
    public CreateModuleMapper()
    {
        CreateMap<CreateModuleRequest, Module>();
        CreateMap<Module, CreateModuleResponse>();
    }
}