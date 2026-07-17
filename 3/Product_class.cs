using System;

class Product
{
    public string Producer {get; set;}

    public double Price {get; set;}

    public string ExpirationDate {get; set;}

    public string ProductionDate {get; set;}

    public Product(string producer, double price, string expirationDate, string productionDate)
    {
        Producer = producer;
        Price = price;
        ExpirationDate = expirationDate;
        ProductionDate = productionDate;
    }


    public void PrintInfo()
    {
        Console.WriteLine($"Producer: {Producer}");
        Console.WriteLine($"Price: {Price}");
        Console.WriteLine($"Expiration date: {ExpirationDate}");
        Console.WriteLine($"Production date: {ProductionDate}");
    }

}

class Program
{
 

    static void Main()
    {
        Product product = new Product("OOO Roga i kopita", 34.44, "03.02.2027", "27.01.2027");
        product.PrintInfo();
    }

}

