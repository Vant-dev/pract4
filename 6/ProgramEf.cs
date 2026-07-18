using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString = config.GetConnectionString("DefaultConnection");

Console.WriteLine("=== Entity Framework CRUD для Products ===");
Console.WriteLine("1. Показать все товары");
Console.WriteLine("2. Добавить товар");
Console.WriteLine("3. Обновить цену");
Console.WriteLine("4. Удалить товар");
Console.Write("Выбери действие: ");
string choice = Console.ReadLine();

switch (choice)
{
    case "1": GetAllProductsEF(); break;
    case "2": AddProductEF(); break;
    case "3": UpdateProductEF(); break;
    case "4": DeleteProductEF(); break;
}

// === EF МЕТОДЫ ===

void GetAllProductsEF()
{
    using var db = new AppDbContext(connectionString);
    var products = db.Products
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .OrderByDescending(p => p.Price)
        .ToList();

    Console.WriteLine("\nТовары в базе:");
    foreach (var p in products)
    {
        Console.WriteLine($"{p.Name} | {p.ModelNumber} | {p.Price:C} | {p.Category?.Name} | {p.Brand?.Name}");
    }
}

void AddProductEF()
{
    Console.Write("Название: ");
    string name = Console.ReadLine();
    Console.Write("Модель: ");
    string model = Console.ReadLine();
    Console.Write("Цена: ");
    decimal price = decimal.Parse(Console.ReadLine());

    using var db = new AppDbContext(connectionString);
    
    // Для простоты берём первую попавшуюся категорию и бренд
    var category = db.Categories.FirstOrDefault();
    var brand = db.Brands.FirstOrDefault();
    
    if (category == null || brand == null)
    {
        Console.WriteLine("Нет категорий или брендов в базе!");
        return;
    }

    var product = new Product
    {
        ProductId = Guid.NewGuid(),
        Name = name,
        ModelNumber = model,
        Price = price,
        IsAvailable = true,
        CreatedAt = DateTime.UtcNow,
        CategoryId = category.CategoryId,
        BrandId = brand.BrandId
    };

    db.Products.Add(product);
    db.SaveChanges();
    Console.WriteLine("Товар добавлен!");
}

void UpdateProductEF()
{
    Console.Write("Введи ModelNumber товара для обновления: ");
    string model = Console.ReadLine();
    Console.Write("Новая цена: ");
    decimal newPrice = decimal.Parse(Console.ReadLine());

    using var db = new AppDbContext(connectionString);
    var product = db.Products.FirstOrDefault(p => p.ModelNumber == model);
    
    if (product != null)
    {
        product.Price = newPrice;
        db.SaveChanges();
        Console.WriteLine("Цена обновлена!");
    }
    else
    {
        Console.WriteLine("Товар не найден");
    }
}

void DeleteProductEF()
{
    Console.Write("Введи ModelNumber товара для удаления: ");
    string model = Console.ReadLine();

    using var db = new AppDbContext(connectionString);
    var product = db.Products.FirstOrDefault(p => p.ModelNumber == model);
    
    if (product != null)
    {
        db.Products.Remove(product);
        db.SaveChanges();
        Console.WriteLine("Товар удалён!");
    }
    else
    {
        Console.WriteLine("Товар не найден");
    }
}
