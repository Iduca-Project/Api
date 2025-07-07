using Iduca.Domain.Models;

namespace Iduca.Application.Repository.ExerciseRepository;

public interface IExerciseRepository : IBaseRepository<Exercise>
{ 
    Task<Exercise?> GetById(Guid id, CancellationToken cancellationToken);    
}