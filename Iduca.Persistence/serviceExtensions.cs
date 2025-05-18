using Iduca.Application.Repositories;
using Iduca.Persistence.Context;
using Iduca.Persistence.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Iduca.Application.Config;
using Microsoft.EntityFrameworkCore;


namespace Iduca.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services)
    {
        DotEnv.Load();

        var connection = DotEnv.Get("DATABASE_URL");

        services.AddDbContext<IducaContext>(opt => opt.UseMySql(connection, ServerVersion.AutoDetect(connection)));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAlternativeRepository, AlternativeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IUserCourseRepository, UserCourseRepository>();
    }
}