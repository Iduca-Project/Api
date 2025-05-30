
using Iduca.Domain.Models;
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ModuleRepository;
using MediatR;

namespace Iduca.Application.Features.Module_.GetById;

public class GetByIdModule(
    IUnitOfWork unitOfWork,
    IModuleRepository moduleRepository,
    IMapper mapper
) : IRequestHandler<GetByIdModuleRequest, GetByIdModuleResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IModuleRepository moduleRepository = moduleRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetByIdModuleResponse> Handle(GetByIdModuleRequest request, CancellationToken cancellationToken)
    {
        Module? findModule = await moduleRepository.Get(request.Id, cancellationToken);
        return mapper.Map<GetByIdModuleResponse>(findModule);
    }
}
