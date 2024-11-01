// See https://aka.ms/new-console-template for more information
using Northwind.Data;
using Northwind.Models;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Windows.Forms;
using System.Linq;
using OxyPlot.Axes;

//Application.EnableVisualStyles();
//Application.SetCompatibleTextRenderingDefault(false);

using (var context = new NorthwindContext())
{
    Console.WriteLine("Hello, World!");
    Console.WriteLine("Enter which Northwind Query you want to see: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.WriteLine("ListEmployees: ");
            ListEmployees(context);
            break;
        case "2":
            Console.WriteLine("ListProductsDescPrice: ");
            ListProductsDescPrice(context);
            break;
        case "3":
            Console.WriteLine("ListCustomersFromSpainOrUk: ");
            ListCustomersFromSpainOrUk(context);
            break;
        case "4":
            Console.WriteLine("ListProductsWithStockOver100: ");
            ListProductsWithStockOver100(context);
            break;
        case "5":
            Console.WriteLine("ListShipCountriesWithOrderOnce: ");
            ListShipCountriesWithOrderOnce(context);
            break;
        case "6":
            Console.WriteLine("ListOrdersFromOctober1996: ");
            ListOrdersFromOctober1996(context);
            break;
        case "7":
            Console.WriteLine("ListTotalSalesFromEachDistinctCountry: ");
            ListTotalSalesFromEachDistinctCountry(context);
            break;
        case "8":
            Console.WriteLine("PlotCategories: ");
            PlotCategories(context);
            break;
        default:
            break;
    }
}
static void ListEmployees(NorthwindContext context)
{
    var employees = context.Employees.ToList();
    foreach (var employee in employees)
    {
        Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
    }
}
static void ListProductsDescPrice(NorthwindContext context)
{
    var products = context.Products
        .OrderByDescending(p=>p.UnitPrice)
        .ToList();
    foreach( var product in products)
    {
        Console.WriteLine($"Product {product.ProductName} has price: {product.UnitPrice}");
    }
}
static void ListCustomersFromSpainOrUk(NorthwindContext context)
{
    var customers = context.Customers
        .Where(c => c.Country.Equals("Spain") || c.Country.Equals("UK"))
        .ToList();
    foreach (var customer in customers)
    {
        Console.WriteLine($"Customer: {customer.ContactName} in Country: {customer.Country}");
    }
}
static void ListProductsWithStockOver100(NorthwindContext context)
{
    var products = context.Products
        .Where(p => p.UnitPrice >= 25 && p.UnitsInStock > 100)
        .ToList();
    foreach (var product in products)
    {
        Console.WriteLine($"Product: {product.ProductName} with Price: {product.UnitPrice} has Stock: {product.UnitsInStock}");
    }
}
static void ListShipCountriesWithOrderOnce(NorthwindContext context)
{
    var shipCountries = context.Orders
        .Select(o=>o.ShipCountry)
        .Distinct()
        .ToList();
    foreach(var shipCountry in shipCountries)
    {
        Console.WriteLine($"Country: {shipCountry}");
    }
}
static void ListOrdersFromOctober1996(NorthwindContext context)
{
    DateTime startDate = new DateTime(1996, 10, 1);
    DateTime endDate = new DateTime(1996, 10, 31);

    var orders = context.Orders
        .Where(o=> o.OrderDate >= startDate && o.OrderDate <= endDate)
        .ToList();
    foreach (var order in orders)
    {
        Console.WriteLine($"{order.ShipName}");
    }
}
static void ListTotalSalesFromEachDistinctCountry(NorthwindContext context)
{
    var countrySales = context.Orders
        .Join(context.Orderdetails,
        o=>o.OrderId, od=>od.OrderId,
        (o,od)=> new {o.ShipCountry, od.UnitPrice, od.Quantity })
        .GroupBy(x=>x.ShipCountry)
        .Select(g=> new
        {
            ShipCountry = g.Key,
            TotalSales = g.Sum(x=>x.UnitPrice*x.Quantity)
        })
        .OrderBy(x=>x.ShipCountry)
        .ToList();
    foreach (var country in countrySales)
    {
        Console.WriteLine($"{country.ShipCountry} {country.TotalSales}");
    }
}
static void PlotCategories(NorthwindContext context)
{
    var categoryCounts = context.Categories
        .Select(c=> new
        {
            CategoryName = c.CategoryName,
            ProductCount = c.Products.Count(p=>p.CategoryId == c.CategoryId)
        })
        .ToList();

    var model = new PlotModel { Title = "Products per Category" };
    var barSeries = new BarSeries
    {
        Title = "Products"
    };
    foreach (var category in categoryCounts)
    {
        barSeries.Items.Add(new BarItem { Value = category.ProductCount });
    }
    model.Series.Add(barSeries);

    var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
    foreach (var category in categoryCounts)
    {
        categoryAxis.Labels.Add(category.CategoryName);
    }
    model.Axes.Add(categoryAxis);
}