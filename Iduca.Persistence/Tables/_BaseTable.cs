using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Iduca.Domain.Models;

namespace Iduca.Persistence.Tables;

public static class BaseTableConfigurationExtensios
{
    public static void ConfigurBaseTableProps<T>(this EntityTypeBuilder<T> builder)
        where T : BaseModel
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(t => t.DisabledAt)
            .HasColumnName("disabled_at");
    }
}