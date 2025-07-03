using MediatR;

namespace Iduca.Application.Features.Analytics.CompanyStats;

public sealed record GetCompanyStatsRequest(
    Guid CompanyId,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IRequest<GetCompanyStatsResponse>;
