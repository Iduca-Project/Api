using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Companies.Create;

public class CreateCompanyHandler (
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CreateCompanyRequest, CreateCompanyResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CreateCompanyResponse> Handle(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = mapper.Map<Company>(request);

        companyRepository.Create(company);

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateCompanyResponse>(company);
    }
}