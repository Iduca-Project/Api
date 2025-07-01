using Microsoft.EntityFrameworkCore;
using Iduca.Persistence.Tables;
using Iduca.Domain.Models;

namespace Iduca.Persistence.Context;

public class IducaContext(DbContextOptions<IducaContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Alternative> Alternatives { get; set; }
    public DbSet<UserCourse> UserCourses { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<Exam> Exams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureAlternativeTable();
        modelBuilder.ConfigureCategoryTable();
        modelBuilder.ConfigureCompanyTable();
        modelBuilder.ConfigureContentTable();
        modelBuilder.ConfigureCourseTable();
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