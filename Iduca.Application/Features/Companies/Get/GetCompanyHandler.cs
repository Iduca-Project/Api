using AutoMapper;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Companies.Get;

public class GetCompanyHandler (
    ICompanyRepository companyRepository,
    IMapper mapper
) : IRequestHandler<GetCompanyRequest, GetCompanyResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetCompanyResponse> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = mapper.Map<Company>(request);

        var findCompany = await companyRepository.GetCompanyByName(company.Name, cancellationToken);

        if (findCompany is null)
            return mapper.Map<GetCompanyResponse>(null);

        return mapper.Map<GetCompanyResponse>(findCompany);
    }
}