using Iduca.Domain.Models;
using Iduca.Application.Features.Lessons.GetByModuleByUser;

namespace Iduca.Application.Repository.LessonRepository;

public interface ILessonRepository : IBaseRepository<Lesson>
{
    Task<Lesson?> GetLessonByEqualName(string title, CancellationToken cancellationToken);    
    Task<List<Lesson>> GetLessonByModuleId(Guid moduleId, CancellationToken cancellationToken);    
    Task<List<UserLesson>> GetLessonByModuleIdAndUser(Guid moduleId, Guid userId, CancellationToken cancellationToken);    
}