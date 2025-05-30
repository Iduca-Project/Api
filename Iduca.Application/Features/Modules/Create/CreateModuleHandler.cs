using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Modules.Create;

public class CreateModule(
    IUnitOfWork unitOfWork,
    IModuleRepository moduleRepository,
    ICourseRepository courseRepository,
    IMapper mapper
) : IRequestHandler<CreateModuleRequest, CreateModuleResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IModuleRepository moduleRepository = moduleRepository;
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IMapper mapper = mapper;

    public async Task<CreateModuleResponse> Handle(CreateModuleRequest request, CancellationToken cancellationToken)
    {
        Module module = mapper.Map<Module>(request);

        Module? findModule = await moduleRepository.GetModuleByEqualNameInCourse(request.Name, request.CourseId, cancellationToken);
        if (findModule is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.ModuleNameDuplicity);

        int lastIndex = await moduleRepository.GetLastModuleIndexInCourse(request.CourseId, cancellationToken);
        module.Index = lastIndex + 1;

        moduleRepository.Create(module);
        await unitOfWork.Save(cancellationToken);
        return mapper.Map<CreateModuleResponse>(module);
    }
}