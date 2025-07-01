using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;

namespace Iduca.Application.Features.Modules.Update;

public class UpdateModuleHandler(
    IModuleRepository moduleRepository,
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<UpdateModuleRequest, UpdateModuleResponse>
{
    private readonly IModuleRepository moduleRepository = moduleRepository;
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<UpdateModuleResponse> Handle(UpdateModuleRequest request, CancellationToken cancellationToken)
    {
        var module = await moduleRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException("Módulo não encontrado.");

        var course = await courseRepository.Get(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Curso não encontrado.");

        module.Name = request.Name;
        module.Description = request.Description;
        module.Index = request.Index;
        module.Course = course;
        module.CourseId = request.CourseId;
        module.UpdatedAt = DateTime.UtcNow;

        moduleRepository.Update(module);
        await unitOfWork.Save(cancellationToken);

        return mapper.Map<UpdateModuleResponse>(module);
    }
}
