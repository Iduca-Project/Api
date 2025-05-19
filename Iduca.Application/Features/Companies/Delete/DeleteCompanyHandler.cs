using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Companies.Delete;

public class DeleteCompanyHandler (
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteCompanyRequest, DeleteCompanyResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<DeleteCompanyResponse> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken)
    {
        var getCompany = await companyRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException();

        companyRepository.Delete(getCompany);

        await unitOfWork.Save(cancellationToken);

        return new DeleteCompanyResponse();
    }
}