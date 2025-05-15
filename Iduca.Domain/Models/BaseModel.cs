namespace Iduca.Domain.Models;

public class BaseModel
{
    public Guid id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DisabledAt { get; set; } = null;

}