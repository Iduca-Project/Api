using Iduca.Domain.Models;

namespace Iduca.Application.Repository.LessonRepository;

public interface ILessonRepository : IBaseRepository<Lesson>
{
    Task<Lesson?> GetLessonByEqualName(string title, CancellationToken cancellationToken);    
    Task<List<Lesson>> GetLessonByModuleId(Guid moduleId, CancellationToken cancellationToken);    
}