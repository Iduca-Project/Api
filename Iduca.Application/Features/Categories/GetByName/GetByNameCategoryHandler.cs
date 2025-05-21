using AutoMapper;
using Iduca.Application.Repository.CategoryRepository;
using MediatR;

namespace Iduca.Application.Features.Categories.GetByName;

public class GetCategoryByName(
    ICategoryRepository categoryRepository,
    IMapper mapper
) : IRequestHandler<GetByNameCategoryRequest, GetByNameCategoryResponse>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IMapper mapper = mapper;
    public async Task<GetByNameCategoryResponse> Handle(GetByNameCategoryRequest request, CancellationToken cancellationToken)
    {
        var findCategories = await categoryRepository.GetCategoriesBySimilarName(request.Name, cancellationToken);
        return mapper.Map<GetByNameCategoryResponse>(findCategories);
    }
}