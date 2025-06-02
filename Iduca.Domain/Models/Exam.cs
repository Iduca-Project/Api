namespace Iduca.Domain.Models;

public class Exam : BaseModel
{
    public required string Title {get;set;}
    public required string Description {get;set;}
    public required DateTime Date {get;set;}
    public required Course Course {get;set;}
    public required Guid CourseId { get; set; }
    public List<Question> Questions { get; set; } = [];
}