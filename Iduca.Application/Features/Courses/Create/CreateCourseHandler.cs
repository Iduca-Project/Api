using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Domain.Models;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;
using Iduca.Application.Features.Companies.Create;
using System.Xml.Linq;
using Iduca.Application.Repository.CategoryRepository;

namespace Iduca.Application.Features.Courses.Create;

public class CreateCourseHandler (
    ICourseRepository courseRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CreateCourseRequest, CreateCourseResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CreateCourseResponse> Handle(CreateCourseRequest request, CancellationToken cancellationToken)
    {
        var course = mapper.Map<Course>(request);

        var findCourse = await courseRepository.GetCourseByEqualName(request.Name, cancellationToken);

        if (findCourse is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.Default);

        var categoryIds = request.Categories.Select(c => c.Id).ToList();
        var categoriesFromDb = new List<Category>();
        foreach (var id in categoryIds)
        {
            var findCategory = await categoryRepository.Get(id, cancellationToken)
                ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);
            categoriesFromDb.Add(findCategory);
        }

        course.Categories = categoriesFromDb;

        courseRepository.Create(course);

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateCourseResponse>(course);
    }
}