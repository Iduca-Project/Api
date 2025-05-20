using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Domain.Models;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;
using Iduca.Application.Features.Companies.Create;

namespace Iduca.Application.Features.Courses.Create;

public class CreateCourseHandler (
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CreateCourseRequest, CreateCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CreateCourseResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
    {
        var course = mapper.Map<Course>(request);

        var findCourse = await courseRepository.GetCourseByEqualName(request.Name, cancellationToken);

        if (findCourse is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.Default);

        courseRepository.Create(course);

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateCourseResponse>(course);
    }
}