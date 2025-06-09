using AutoMapper;
using Iduca.Application.Repository.CompanyRepository;
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
        var findCompanies = await companyRepository.GetCompanyByName(request.Name, cancellationToken);

        return mapper.Map<GetCompanyResponse>(findCompanies);
    }
}