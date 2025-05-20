using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Categories.Create;

public class CreateCategory(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;
    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = mapper.Map<Category>(request);

        var findCategory = categoryRepository.GetCategoryByEqualName(category.Name, cancellationToken);

        if (findCategory is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.CompanyNameDuplicity);

        categoryRepository.Create(category);
        await unitOfWork.Save(cancellationToken);
        return mapper.Map<CreateCategoryResponse>(category);
    }
}

