
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Module_.DeleteById;

public class DeleteByIdModuleMapper : Profile
{
    public DeleteByIdModuleMapper()
    {
        CreateMap<DeleteByIdModuleRequest, Module>();
        CreateMap<Module, DeleteByIdModuleResponse>();
    }
}
