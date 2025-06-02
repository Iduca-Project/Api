using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Modules.Create;

public class GetByCourseIdModule(
    IModuleRepository moduleRepository,
    IMapper mapper
) : IRequestHandler<GetByCourseIdModuleRequest, GetByCourseIdModuleResponse>
{
    private readonly IModuleRepository moduleRepository = moduleRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetByCourseIdModuleResponse> Handle(GetByCourseIdModuleRequest request, CancellationToken cancellationToken)
    {
        List<Module> findModules = await moduleRepository.GetModuleByCourseId(request.CourseId, cancellationToken);
        return mapper.Map<GetByCourseIdModuleResponse>(findModules);
    }
}