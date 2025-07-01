using AutoMapper;
using Iduca.Application.Repository.CategoryRepository;
using MediatR;

namespace Iduca.Application.Features.Categories.GetAll;

public class GetAllCategoriesHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper
) : IRequestHandler<GetAllCategoriesRequest, List<GetAllCategoriesResponse>>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IMapper mapper = mapper;

    public async Task<List<GetAllCategoriesResponse>> Handle(GetAllCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAll(cancellationToken);
        return mapper.Map<List<GetAllCategoriesResponse>>(categories);
    }
}
