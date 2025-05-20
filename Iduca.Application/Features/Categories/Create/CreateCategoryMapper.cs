using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Categories.Create;

public class CreateCategoryMapper : Profile
{
    public CreateCategoryMapper()
    {
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<Category, CreateCategoryResponse>();
    }
}