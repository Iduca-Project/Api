using AutoMapper;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.CourseRepository;
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
        var findCompanies = await courseRepository.GetCompanyByName(request.Name, cancellationToken);

        return mapper.Map<GetCoursesResponse>(findCompanies);
    }
}