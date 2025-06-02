using Iduca.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Iduca.Persistence.Tables;

public static class ExamClassMap
{
    public static void ConfigureExamTable(this ModelBuilder modelBuilder)
        => modelBuilder.Entity<Exam>(builder =>
    {
        builder.ConfigurBaseTableProps();

        builder.HasKey(exam => exam.Id)
            .HasName("exam_id");

        builder.ToTable("tb_exam");

        builder.Property(e => e.Title)
            .HasMaxLength(64)
            .HasColumnName("title");

        builder.Property(e => e.Description)
            .HasMaxLength(511)
            .HasColumnName("description");

        builder.Property(e => e.Date)
            .HasColumnName("date");

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");
    });
}