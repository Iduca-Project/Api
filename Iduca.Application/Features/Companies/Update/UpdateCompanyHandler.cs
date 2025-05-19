using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Companies.Update;

public class UpdateCompanyHandler (
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<UpdateCompanyRequest, UpdateCompanyResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<UpdateCompanyResponse> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);

        company.Name = request.Props.NewName;

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<UpdateCompanyResponse>(company);
    }
}