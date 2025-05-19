using AutoMapper;
using Iduca.Application.Repository.CompanyRepository;
using MediatR;

namespace Iduca.Application.Features.Companies.GetAll;

public class GetAllCompanyHandler (
    ICompanyRepository companyRepository,
    IMapper mapper
) : IRequestHandler<GetAllCompanyRequest, List<GetAllCompanyResponse>>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IMapper mapper = mapper;

    public async Task<List<GetAllCompanyResponse>> Handle(GetAllCompanyRequest request, CancellationToken cancellationToken)
    {
        var findCompanies = await companyRepository.GetAll(cancellationToken);

        return mapper.Map<List<GetAllCompanyResponse>>(findCompanies);
    }
}