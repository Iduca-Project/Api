using Iduca.Application.Repository.ExamRepository;
using Iduca.Domain.Models;
using Iduca.Persistence.Context;

namespace Iduca.Persistence.Repositories.Courses;

public class ExamRepository(IducaContext context)
    : BaseRepository<Exam>(context), IExamRepository
{    
}
