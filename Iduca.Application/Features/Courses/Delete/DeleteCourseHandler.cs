using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Features.Courses.Delete;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using MediatR;

namespace Iduca.Application.Features.Companies.Delete;

public class DeleteCourseHandler (
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteCourseRequest, DeleteCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<DeleteCourseResponse> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
    {
        var getCompany = await courseRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException();

        courseRepository.Delete(getCompany);

        await unitOfWork.Save(cancellationToken);

        return new DeleteCourseResponse();
    }
}