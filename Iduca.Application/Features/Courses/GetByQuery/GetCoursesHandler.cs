using System.Text.Json.Nodes;
using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.GetByQuery;

public class GetCoursesHandler (
    ICourseRepository courseRepository,
    IMapper mapper
) : IRequestHandler<GetCoursesRequest, GetCoursesResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetCoursesResponse> Handle(GetCoursesRequest request, CancellationToken cancellationToken)
    {
    
        var findCourses = await courseRepository.GetCoursesByQuery(request.Name, request.Difficulty, request.Categories, request.Page, request.MaxItens, cancellationToken)
            ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);

        return mapper.Map<GetCoursesResponse>(findCourses);
    }
}