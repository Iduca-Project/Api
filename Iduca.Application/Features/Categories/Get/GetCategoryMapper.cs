using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Categories.Get;

public class GetCategoryMapper : Profile
{
    public GetCategoryMapper()
    {
        CreateMap<Category, GetCategoryResponse>();
    }
}
