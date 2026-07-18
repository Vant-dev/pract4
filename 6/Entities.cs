using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Products")]
public class Product
{
    [Key]
    public Guid ProductId { get; set; }
    
    [Required, MaxLength(200)]
    public string Name { get; set; }
    
    [Required, MaxLength(50)]
    public string ModelNumber { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    public DateTime? ReleaseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAvailable { get; set; }
    
    // Внешние ключи
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    
    // Навигационные свойства
    public Category Category { get; set; }
    public Brand Brand { get; set; }
}

[Table("Categories")]
public class Category
{
    [Key]
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // У категории много продуктов
    public ICollection<Product> Products { get; set; }
}

[Table("Brands")]
public class Brand
{
    [Key]
    public Guid BrandId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    
    public ICollection<Product> Products { get; set; }
}
