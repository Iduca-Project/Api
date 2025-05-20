using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CategoryRepository;
using MediatR;

namespace Iduca.Application.Features.Categories.DeleteById;

public class DeleteByIdCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<DeleteByIdCategoryRequest, DeleteByIdCategoryResponse>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<DeleteByIdCategoryResponse> Handle(DeleteByIdCategoryRequest request, CancellationToken cancellationToken)
    {
        var findCategory = await categoryRepository.Get(request.Id, cancellationToken)
        ?? throw new NotFoundException();

        categoryRepository.Delete(findCategory);

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<DeleteByIdCategoryResponse>(findCategory);
    }
}