using MediatR;

namespace Iduca.Application.Features.Analytics.CategoryStats;

public sealed record GetCategoryStatsRequest(
    Guid CategoryId,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IRequest<GetCategoryStatsResponse>;
