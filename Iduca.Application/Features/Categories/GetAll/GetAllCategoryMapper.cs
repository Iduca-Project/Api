
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Categories.GetAll;

public class GetAllCategoryMapper : Profile
{
    public GetAllCategoryMapper()
    {
        CreateMap<GetAllCategoryRequest, Category>();
        CreateMap<Category, GetAllCategoryResponse>();
    }
}
