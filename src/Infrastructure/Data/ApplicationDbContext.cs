using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Course> Modules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your entity relationships and constraints here if needed.
        // Example:
        // modelBuilder.Entity<Student>()
        //     .HasOne(s => s.Parent)
        //     .WithMany(p => p.Students)
        //     .HasForeignKey(s => s.ParentId);


        // Example of setting a unique constraint (e.g., email)
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();

        //modelBuilder.Entity<Admin>()
        //    .HasIndex(a => a.Email)
        //    .IsUnique();

        //modelBuilder.Entity<Parent>()
        //    .HasIndex(p => p.Email)
        //    .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
