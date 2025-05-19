using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Models;
using Iduca.Domain.Common.Messages;
using Iduca.Application.Common.Exceptions;
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

        var findCompany = await companyRepository.GetCompanyByName(company.Name, cancellationToken);

        if (findCompany is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.CompanyNameDuplicity);

        companyRepository.Create(company);

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateCompanyResponse>(company);
    }
}