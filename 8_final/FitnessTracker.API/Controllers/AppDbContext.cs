using Microsoft.EntityFrameworkCore;
using FitnessTracker.API.Models;

namespace FitnessTracker.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TrainingProgram> TrainingPrograms => Set<TrainingProgram>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrainingProgram>().ToTable("TrainingPrograms");
        modelBuilder.Entity<Exercise>().ToTable("Exercises");
        modelBuilder.Entity<Activity>().ToTable("Activities");

        modelBuilder.Entity<Exercise>()
            .HasOne(e => e.Program)
            .WithMany(p => p.Exercises)
            .HasForeignKey(e => e.ProgramId);

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Exercise)
            .WithMany(e => e.Activities)
            .HasForeignKey(a => a.ExerciseId);

        modelBuilder.Entity<Activity>()
            .HasIndex(a => a.Date);
    }
}
