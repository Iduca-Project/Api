
using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Domain.Common.Messages;


using Iduca.Application.Repository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Categories.GetAll;

public class GetAllCategory(
    ICategoryRepository categoryRepository,
    IMapper mapper
) : IRequestHandler<GetAllCategoryRequest, GetAllCategoryResponse>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetAllCategoryResponse> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetByQuery(request.Name, request.Page, request.MaxItens, cancellationToken);

        return new GetAllCategoryResponse(
            Categories: mapper.Map<List<CategoryDto>>(categories)
        );
    }
}
