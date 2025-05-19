using AutoMapper;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Companies.GetAll;

public class GetCompanyHandler (
    ICompanyRepository companyRepository,
    IMapper mapper
) : IRequestHandler<GetAllCompanyRequest, GetAllCompanyResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetAllCompanyResponse> Handle(GetAllCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = mapper.Map<Company>(request);

        var findCompany = await companyRepository.GetCompanyByName(company.Name, cancellationToken);

        if (findCompany is null)
            return mapper.Map<GetAllCompanyResponse>(null);

        return mapper.Map<GetAllCompanyResponse>(findCompany);
    }
}