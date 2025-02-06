using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Course> Modules { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}