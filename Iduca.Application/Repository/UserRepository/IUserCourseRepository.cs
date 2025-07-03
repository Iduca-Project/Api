using Iduca.Domain.Models;

namespace Iduca.Application.Repository.UserCourseRepository;

public interface IUserCourseRepository : IBaseRepository<UserCourse>
{
    Task<List<UserCourse>> GetAllByCourseId(Guid courseId, CancellationToken cancellationToken);
    Task<UserCourse?> GetUserCourseByIds(Guid userId, Guid courseId, CancellationToken cancellationToken);
    Task<List<UserCourse>> GetAllByUserId(Guid userId, CancellationToken cancellationToken);
    Task<int> GetCompletedLessonsCount(Guid userId, Guid courseId, CancellationToken cancellationToken);
    Task<List<Guid>> GetCompletedLessonIds(Guid userId, Guid courseId, CancellationToken cancellationToken);
    Task<List<UserCourse>> GetAllByCompanyId(Guid companyId, CancellationToken cancellationToken);
}