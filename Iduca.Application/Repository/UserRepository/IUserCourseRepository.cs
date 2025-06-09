using Iduca.Domain.Models;

namespace Iduca.Application.Repository.UserCourseRepository;

public interface IUserCourseRepository : IBaseRepository<UserCourse>
{
    public Task<List<UserCourse>> GetAllByCourseId(Guid courseId, CancellationToken cancellationToken);
}