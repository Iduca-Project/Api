using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;

namespace Iduca.Application.Repository.CourseRepository;

public interface ICourseRepository : IBaseRepository<Course>
{
    Task<List<Course>> GetCourseByName(string name, CancellationToken cancellationToken);
    public Task<Course?> GetCourseByEqualName(string name, CancellationToken cancellationToken);
    public Task<List<Course>> GetCoursesByQuery(string? name, CourseDifficulty? difficulty, Guid? category, CancellationToken cancellationToken);
}