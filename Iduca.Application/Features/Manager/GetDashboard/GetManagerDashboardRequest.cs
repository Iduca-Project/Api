using MediatR;

namespace Iduca.Application.Features.Manager.GetDashboard;

public sealed record GetManagerDashboardRequest(
    Guid ManagerId
) : IRequest<GetManagerDashboardResponse>;
