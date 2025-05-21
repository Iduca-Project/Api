using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Categories.GetByName;

public class GetByNameCategoryMapper : Profile
{
    public GetByNameCategoryMapper()
    {
        CreateMap<GetByNameCategoryRequest, Category>();
        CreateMap<Category, GetByNameCategoryResponse>();
    }
}