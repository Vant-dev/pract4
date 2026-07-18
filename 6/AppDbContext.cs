using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    
    private readonly string _connectionString;
    
    public AppDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связей и ограничений
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId);
            
            entity.HasOne(p => p.Brand)
                  .WithMany(b => b.Products)
                  .HasForeignKey(p => p.BrandId);
            
            entity.HasIndex(p => p.ModelNumber).IsUnique();
            entity.Property(p => p.Price).HasColumnType("decimal(10,2)");
            entity.Property(p => p.IsAvailable).HasDefaultValue(true);
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
        });
    }
}
