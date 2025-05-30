
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Module_.GetById;

public class GetByIdModuleMapper : Profile
{
    public GetByIdModuleMapper()
    {
        CreateMap<GetByIdModuleRequest, Module>();
        CreateMap<Module, GetByIdModuleResponse>();
    }
}
