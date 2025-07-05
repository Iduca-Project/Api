using MediatR;

namespace Iduca.Application.Features.Manager.EnrollEmployee;

public class EnrollEmployeeRequest : IRequest<EnrollEmployeeResponse>
{
    public required Guid EmployeeId { get; set; }
    public required Guid CourseId { get; set; }
}
