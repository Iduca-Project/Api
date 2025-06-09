using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.Update;

public class UpdateCourseHandler (
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<UpdateCourseRequest, UpdateCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<UpdateCourseResponse> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);

        var courseByName = await courseRepository.GetCourseByEqualName(request.Name, cancellationToken);

        if (courseByName is not null && courseByName.Id != course.Id)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.Default);

        course.Name = request.Name;
        course.Description = request.Description;
        course.Difficulty = (int)request.Difficulty;
        course.Image = request.Image;
        course.TotalHours = request.TotalHours;
        // course.Categories = categories; ! Necessário fazer essa parte ainda!

        var students = userCourseRepository.GetAllByCourseId(course.Id, cancellationToken);

        await unitOfWork.Save(cancellationToken);

        return new UpdateCourseResponse(
            course.Name,    
            course.Id,
            course.UpdatedAt,
            course.Categories,
            course.Modules,
            students.Result.Count
        );
    }
}