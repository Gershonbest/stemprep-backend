using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> User { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}