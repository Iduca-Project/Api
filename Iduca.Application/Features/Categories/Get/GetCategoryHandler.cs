using AutoMapper;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
using MediatR;

namespace Iduca.Application.Features.Categories.Get;

public class GetCategoryHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper
) : IRequestHandler<GetCategoryRequest, GetCategoryResponse>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetCategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException("Categoria não encontrada.");

        return mapper.Map<GetCategoryResponse>(category);
    }
}
