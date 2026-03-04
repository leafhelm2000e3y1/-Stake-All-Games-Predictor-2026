using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SalesManagementSystem
{
    // Модели данных
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<SaleItem> Items { get; set; } = new();
        public int CustomerId { get; set; }
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
    }

    public class SaleItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
    }

    // Основная система управления продажами
    public class SalesManager
    {
        private List<Product> _products = new();
        private List<Customer> _customers = new();
        private List<Sale> _sales = new();
        private int _nextProductId = 1;
        private int _nextCustomerId = 1;
        private int _nextSaleId = 1;
        private const string DataFile = "sales_data.json";

        public SalesManager()
        {
            LoadData();
        }

        // Запуск приложения
        public void Run()
        {
            DisplayWelcome();

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": AddProduct(); break;
            case "2": AddCustomer(); break;
            case "3": CreateSale(); break;
            case "4": ViewAllProducts(); break;
            case "5": ViewAllCustomers(); break;
            case "6": ViewAllSales(); break;
            case "7": ViewSalesReport(); break;
            case "8": SaveAndExit(); return;
            default:
                Console.WriteLine("Invalid choice. Please enter 1–8.
");
                break;
        }
    }
}

private void DisplayWelcome()
{
    Console.Clear();
    Console.WriteLine("=================================================");
    Console.WriteLine("      SALES MANAGEMENT SYSTEM");
    Console.WriteLine("=================================================");
    Console.WriteLine();
}

private void DisplayMenu()
{
    Console.WriteLine("
MAIN MENU:");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. Add Customer");
    Console.WriteLine("3. Create Sale");
    Console.WriteLine("4. View All Products");
    Console.WriteLine("5. View All Customers");
    Console.WriteLine("6. View All Sales");
    Console.WriteLine("7. Sales Report");
    Console.WriteLine("8. Save and Exit");
    Console.Write("
Enter your choice (1–8): ");
}

// Добавление товара
private void AddProduct()
{
    Console.WriteLine("
--- ADD NEW PRODUCT ---");
    Console.Write("Product name: ");
    string name = Console.ReadLine() ?? string.Empty;

    decimal price = GetDecimalInput("Price: ");
    int stock = GetIntInput("Stock quantity: ");
    Console.Write("Category: ");
    string category = Console.ReadLine() ?? "General";

    _products.Add(new Product
    {
        Id = _nextProductId++,
        Name = name,
        Price = price,
        StockQuantity = stock,
        Category = category
    });
    Console.WriteLine($"Product '{name}' added successfully.
");
}

// Добавление клиента
private void AddCustomer()
{
    Console.WriteLine("
--- ADD NEW CUSTOMER ---");
    Console.Write("Customer name: ");
    string name = Console.ReadLine() ?? string.Empty;
    Console.Write("Email: ");
    string email = Console.ReadLine() ?? string.Empty;
    Console.Write("Phone: ");
    string phone = Console.ReadLine() ?? string.Empty;

    _customers.Add(new Customer
    {
        Id = _nextCustomerId++,
        Name = name,
        Email = email,
        Phone = phone
    });
    Console.WriteLine($"Customer '{name}' added successfully.
");
}

// Создание продажи
private void CreateSale()
{
    Console.WriteLine("
--- CREATE NEW SALE ---");

    int customerId = GetIntInput("Customer ID: ");
    var customer = _customers.FirstOrDefault(c => c.Id == customerId);
    if (customer == null)
    {
        Console.WriteLine("Customer not found.
");
        return;
    }

    var sale = new Sale { Id = _nextSaleId++, CustomerId = customerId };

    Console.WriteLine("Add products to sale (enter 0 to finish):");
    while (true)
    {
        int productId = GetIntInput("Product ID (0 to finish): ");
        if (productId == 0) break;

        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            continue;
        }

        int quantity = GetIntInput($"Quantity (available: {product.StockQuantity}): ");
        if (quantity > product.StockQuantity)
        {
            Console.WriteLine("Not enough stock available.");
            continue;
        }

        sale.Items.Add(new SaleItem
        {
            ProductId = productId,
            ProductName = product.Name,
            UnitPrice = product.Price,
            Quantity = quantity
        });

        product.StockQuantity -= quantity;
    }

    if (sale.Items.Count > 0)
    {
        _sales.Add(sale);
        Console.WriteLine($"Sale created successfully. Total: ${sale.TotalAmount:F2}
");
    }
    else
    {
        Console.WriteLine("Sale cancelled — no items added.
");
    }
}

// Просмотр всех товаров
private void ViewAllProducts()
{
    if (_products.Count == 0)
    {
        Console.WriteLine("No products found.
");
        return;
    }

    Console.WriteLine("
=== ALL PRODUCTS ===");
    foreach (var product in _products)
    {
        Console.WriteLine($"{product.Id}. {product.Name} - ${product.Price:F2}");
        Console.WriteLine($"   Stock: {product.StockQuantity}, Category: {product.Category}");
    }
    Console.WriteLine();
}

// Просмотр всех клиентов
private void ViewAllCustomers()
{
    if (_customers.Count == 0)
    {
        Console.WriteLine("No customers found.
");
        return;
    }

    Console.WriteLine("
=== ALL CUSTOMERS ===");
    foreach (var customer in _customers)
    {
        Console.WriteLine($"{customer.Id}. {customer.Name}");
        Console.WriteLine($"   Email: {customer.Email}, Phone: {customer.Phone}");
    }
    Console.WriteLine();
}

// Просмотр всех продаж
private void ViewAllSales()
{
    if (_sales.Count == 0)
    {
        Console.WriteLine("No sales found.
");
        return;
    }

    Console.WriteLine("
=== ALL SALES ===");
    foreach (var sale in _sales.OrderByDescending(s => s.Date))
    {
        var customer = _customers.FirstOrDefault(c => c.Id == sale.CustomerId);
        string customerName = customer?.Name ?? "Unknown";
        Console.WriteLine($"{sale.Id}. {sale.Date:yyyy
