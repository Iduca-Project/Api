
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Categories.GetAll;

public class GetAllCategory(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    IMapper mapper
) : IRequestHandler<GetAllCategoryRequest, GetAllCategoryResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetAllCategoryResponse> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
    {
        var categories = categoryRepository.GetByQuery(request.Name, request.Page, request.MaxItens, cancellationToken);

        await unitOfWork.Save(cancellationToken);
        return mapper.Map<GetAllCategoryResponse>(categories);
    }
}
