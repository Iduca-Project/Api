using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;

namespace Iduca.Application.Repository.CourseRepository;

public interface ICourseRepository : IBaseRepository<Course>
{
    Task<List<Course>> GetCourseByName(string name, CancellationToken cancellationToken);
    public Task<Course?> GetById(Guid id, CancellationToken cancellationToken);
    public Task<Course?> GetCourseByEqualName(string name, CancellationToken cancellationToken);
    public Task<List<Course>> GetCoursesByQuery(string? name, int? difficulty, Guid? category, int page, int maxItens, CancellationToken cancellationToken);
    Task<List<Course>> GetCoursesByCategory(Guid categoryId, CancellationToken cancellationToken);
    Task<List<Course>> GetCoursesByUserId(Guid userId, CancellationToken cancellationToken);
    Task<Course?> GetWithModulesAndLessons(Guid courseId, CancellationToken cancellationToken);
}