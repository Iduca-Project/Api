using AutoMapper;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Companies.GetById;

public class GetByIdCompanyHandler
(
    ICompanyRepository companyRepository,
    IMapper mapper
) : IRequestHandler<GetByIdCompanyRequest, GetByIdCompanyResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IMapper mapper = mapper;
    public async Task<GetByIdCompanyResponse> Handle(GetByIdCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = mapper.Map<Company>(request);

        var findCompany = await companyRepository.Get(request.Id, cancellationToken);

        if (findCompany is null)
            return mapper.Map<GetByIdCompanyResponse>(null);

        return mapper.Map<GetByIdCompanyResponse>(findCompany);
    }
}