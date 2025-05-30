
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Modules.DeleteById;

public class DeleteByIdModuleMapper : Profile
{
    public DeleteByIdModuleMapper()
    {
        CreateMap<DeleteByIdModuleRequest, Module>();
        CreateMap<Module, DeleteByIdModuleResponse>();
    }
}
