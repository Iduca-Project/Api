using MediatR;

namespace Iduca.Application.Features.Companies.GetAll;

public sealed record GetAllCompanyRequest(

) : IRequest<GetAllCompanyResponse>;