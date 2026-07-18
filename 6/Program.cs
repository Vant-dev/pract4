using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

// Читаем конфиг
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string connectionString = config.GetConnectionString("DefaultConnection");

Console.WriteLine("=== ADO.NET CRUD для Products ===");
Console.WriteLine("1. Показать все товары");
Console.WriteLine("2. Добавить товар");
Console.WriteLine("3. Обновить цену");
Console.WriteLine("4. Удалить товар");
Console.Write("Выбери действие: ");
string choice = Console.ReadLine();

switch (choice)
{
    case "1": GetAllProducts(); break;
    case "2": AddProduct(); break;
    case "3": UpdateProduct(); break;
    case "4": DeleteProduct(); break;
    default: Console.WriteLine("Неверный выбор"); break;
}

// === МЕТОДЫ CRUD ===

void GetAllProducts()
{
    string sql = @"SELECT p.Name, p.ModelNumber, p.Price, c.Name AS Category, b.Name AS Brand
                   FROM Products p
                   JOIN Categories c ON p.CategoryId = c.CategoryId
                   JOIN Brands b ON p.BrandId = b.BrandId
                   ORDER BY p.Price DESC";

    using var connection = new SqlConnection(connectionString);
    connection.Open();

    using var command = new SqlCommand(sql, connection);
    using var reader = command.ExecuteReader();

    Console.WriteLine("\nТовары в базе:");
    while (reader.Read())
    {
        Console.WriteLine($"{reader["Name"]} | {reader["ModelNumber"]} | {reader["Price"]:C} | {reader["Category"]} | {reader["Brand"]}");
    }
}

void AddProduct()
{
    Console.Write("Название: ");
    string name = Console.ReadLine();
    Console.Write("Модель: ");
    string model = Console.ReadLine();
    Console.Write("Цена: ");
    decimal price = decimal.Parse(Console.ReadLine());
    
    // Для простоты — хардкодим CategoryId и BrandId (в реальности — выбор из списка)
    string sql = @"INSERT INTO Products (Name, ModelNumber, Price, CategoryId, BrandId)
                   VALUES (@Name, @Model, @Price, 
                           (SELECT TOP 1 CategoryId FROM Categories),
                           (SELECT TOP 1 BrandId FROM Brands))";

    using var connection = new SqlConnection(connectionString);
    connection.Open();

    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Model", model);
    command.Parameters.AddWithValue("@Price", price);

    int rows = command.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Товар добавлен!" : "Ошибка при добавлении");
}

void UpdateProduct()
{
    Console.Write("Введи ModelNumber товара для обновления: ");
    string model = Console.ReadLine();
    Console.Write("Новая цена: ");
    decimal newPrice = decimal.Parse(Console.ReadLine());

    string sql = "UPDATE Products SET Price = @Price WHERE ModelNumber = @Model";

    using var connection = new SqlConnection(connectionString);
    connection.Open();

    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@Price", newPrice);
    command.Parameters.AddWithValue("@Model", model);

    int rows = command.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Цена обновлена!" : "Товар не найден");
}

void DeleteProduct()
{
    Console.Write("Введи ModelNumber товара для удаления: ");
    string model = Console.ReadLine();

    string sql = "DELETE FROM Products WHERE ModelNumber = @Model";

    using var connection = new SqlConnection(connectionString);
    connection.Open();

    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@Model", model);

    int rows = command.ExecuteNonQuery();
    Console.WriteLine(rows > 0 ? "Товар удалён!" : "Товар не найден");
}
