namespace Iduca.Application.Features.Manager.EnrollEmployee;

public class EnrollEmployeeResponse
{
    public required bool Success { get; set; }
    public required string Message { get; set; }
    public Guid? UserCourseId { get; set; }
}
