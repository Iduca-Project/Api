using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Modules.Create;

public class CreateModule(
    IUnitOfWork unitOfWork,
    IModuleRepository moduleRepository,
    IMapper mapper
) : IRequestHandler<CreateModuleRequest, CreateModuleResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IModuleRepository moduleRepository = moduleRepository;
    private readonly IMapper mapper = mapper;

    public async Task<CreateModuleResponse> Handle(CreateModuleRequest request, CancellationToken cancellationToken)
    {
        Module module = mapper.Map<Module>(request);
        Module? findModule = await moduleRepository.GetModuleByEqualName(request.Name, cancellationToken);
        if (findModule is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.ModuleNameDuplicity);

        moduleRepository.Create(module);
        await unitOfWork.Save(cancellationToken);
        return mapper.Map<CreateModuleResponse>(module);
    }
}