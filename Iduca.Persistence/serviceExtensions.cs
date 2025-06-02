using Iduca.Persistence.Context;
using Iduca.Persistence.Repositories;
using Iduca.Persistence.Repositories.Users;
using Iduca.Persistence.Repositories.Alternatives;
using Iduca.Persistence.Repositories.Categories;
using Iduca.Persistence.Repositories.Companies;
using Iduca.Persistence.Repositories.Contents;
using Iduca.Persistence.Repositories.Courses;
using Iduca.Persistence.Repositories.Exercises;
using Iduca.Persistence.Repositories.Lessons;
using Iduca.Persistence.Repositories.Logs;
using Iduca.Persistence.Repositories.Modules;
using Iduca.Persistence.Repositories.Reminders;

using Iduca.Application.Config;
using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.AlternativeRepository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.ContentRepository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.ExerciseRepository;
using Iduca.Application.Repository.LessonRepository;
using Iduca.Application.Repository.LogRepository;
using Iduca.Application.Repository.ModuleRepository;
using Iduca.Application.Repository.ReminderRepository;
using Iduca.Application.Repository.UserCourseRepository;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Iduca.Application.Repository.ExamRepository;

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
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IUserCourseRepository, UserCourseRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();
    }
}