
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using MediatR;

namespace Iduca.Application.Features.Courses.GetCourseOfUser;

public class GetCourseOfUserCourse(
    IUnitOfWork unitOfWork,
    ICourseRepository courseRepository,
    IMapper mapper
) : IRequestHandler<GetCourseOfUserCourseRequest, GetCourseOfUserCourseResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetCourseOfUserCourseResponse> Handle(GetCourseOfUserCourseRequest request, CancellationToken cancellationToken)
    {

        var courses = await courseRepository.GetCoursesByUserId(request.UserId, cancellationToken);

        await unitOfWork.Save(cancellationToken);
        return mapper.Map<GetCourseOfUserCourseResponse>(courses);
    }
}
