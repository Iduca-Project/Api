using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using MediatR;

namespace Iduca.Application.Features.Courses.GetByQuery;

public class GetCoursesHandler (
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    IMapper mapper
) : IRequestHandler<GetCoursesRequest, GetCoursesResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetCoursesResponse> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
    {
    
        var findCourses = await courseRepository.GetCoursesByQuery(request.Name, request.Difficulty, request.Categories, request.Page, request.MaxItens, cancellationToken)
            ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);

        var coursePropsList = new List<GetCourseProps>();

        foreach (var course in findCourses)
        {
            var userCourses = await userCourseRepository.GetAllByCourseId(course.Id, cancellationToken);

            var studentsCount = userCourses.Count;

            coursePropsList.Add(new GetCourseProps(
                course.Name,
                course.Description,
                (int)course.Difficulty,
                course.Image,
                course.TotalHours,
                studentsCount,
                course.Categories.Select(cat => new CategoryProps(cat.Name)).ToList()
            ));
        }

        return new GetCoursesResponse(coursePropsList);
    }
}