using Iduca.Domain.Models;
using AutoMapper;

namespace Iduca.Application.Features.Categories.GetAll;

public class GetAllCategoriesMapper : Profile
{
    public GetAllCategoriesMapper()
    {
        CreateMap<Category, GetAllCategoriesResponse>();
    }
}
