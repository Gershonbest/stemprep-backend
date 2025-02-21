using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Course> Modules { get; set; }
    public DbSet<Tutor> Tutors { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Admin> Admins { get; set; }

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

        modelBuilder.Entity<Tutor>()
            .HasIndex(a => a.Email)
            .IsUnique();

        modelBuilder.Entity<Parent>()
            .HasIndex(p => p.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
