using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;


namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> User { get; set; }
        
        public DbSet<Course> Modules { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}