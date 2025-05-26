using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using MediatR;

namespace Iduca.Application.Features.Courses.Get;

public class GetCourseHandler (
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    IMapper mapper
) : IRequestHandler<GetCourseRequest, GetCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetCourseResponse> Handle(GetCourseRequest request, CancellationToken cancellationToken)
    {

        var findCourse = await courseRepository.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);

        var userCourses = await userCourseRepository.GetAllByCourseId(findCourse.Id, cancellationToken);

        var studentsCount = userCourses.Count();

        var response = new GetCourseResponse(
            findCourse.Name,
            findCourse.Description,
            (int)findCourse.Difficulty,
            findCourse.Image,
            findCourse.TotalHours,
            studentsCount,
            findCourse.Categories.Select(cat => new CategoryProps(cat.Name)).ToList()
        );

        return response;
    }
}