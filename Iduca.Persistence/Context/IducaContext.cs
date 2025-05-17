using Microsoft.EntityFrameworkCore;
using Iduca.Persistence.Tables;

namespace Iduca.Persistence.Context;

public class IducaContext(DbContextOptions<IducaContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureAlternativeTable();
        modelBuilder.ConfigureCategoryTable();
        modelBuilder.ConfigureCompanyTable();
        modelBuilder.ConfigureContentTable();
        modelBuilder.ConfigureCourseTable();
        modelBuilder.ConfigureExamTable();
        modelBuilder.ConfigureExerciseTable();
        modelBuilder.ConfigureLessonTable();
        modelBuilder.ConfigureLogTable();
        modelBuilder.ConfigureModuleTable();
        modelBuilder.ConfigureQuestionTable();
        modelBuilder.ConfigureReminderTable();
        modelBuilder.ConfigureUserTable();
        modelBuilder.ConfigureUserCourseTable();
    }
}