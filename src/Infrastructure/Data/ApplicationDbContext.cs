using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    IdentityDbContext<BaseUser>(options), IApplicationDbContext 

{
    public DbSet<User> User { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        //foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        //{
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            entry.Entity.CreatedDate = DateTime.Now;
        //            break;
        //        case EntityState.Modified:
        //            entry.Entity.LastModifiedDate = DateTime.Now;
        //            break;
        //    }
        //}
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}