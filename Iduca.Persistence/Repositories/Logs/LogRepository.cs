using Iduca.Application.Repository.LogRepository;
using Iduca.Domain.Models;
using Iduca.Persistence.Context;

namespace Iduca.Persistence.Repositories.Logs;

public class LogRepository(IducaContext context)
    : BaseRepository<Log>(context), ILogRepository
{
}
