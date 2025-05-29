using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Module_.DeleteById;

public class DeleteByIdModule(
    IUnitOfWork unitOfWork,
    IModuleRepository moduleRepository
) : IRequestHandler<DeleteByIdModuleRequest, DeleteByIdModuleResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IModuleRepository moduleRepository = moduleRepository;

    public async Task<DeleteByIdModuleResponse> Handle(DeleteByIdModuleRequest request, CancellationToken cancellationToken)
    {
        Module? findModule = await moduleRepository.Get(request.Id, cancellationToken)
        ?? throw new NotFoundException();
        
        moduleRepository.Delete(findModule);
        await unitOfWork.Save(cancellationToken);
        return new DeleteByIdModuleResponse();
    }
}
