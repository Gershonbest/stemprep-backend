
namespace Domain.Common.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDated { get; set; } = DateTime.UtcNow;
    }
}