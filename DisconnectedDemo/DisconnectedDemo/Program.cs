using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDemo.Data;
using DisconnectedDemo.Models;
using static System.Console;

namespace DisconnectedDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            ProductService service = new ProductService();
            WriteLine("Current Products -----------");
            List<Product> products = service.GetAllProducts();
            Display(products);


            WriteLine("Adding new Product");
            service.AddProduct(new Product(17, "Oven", 1000, "Kitchen Appliance"));

            service.AddProduct(new Product(18, "Laptop", 5000, "Computer Accessories"));
            //Call the adapter's Update method to update the changes in the database
            service.SaveChanges();
            WriteLine("-----------------------------\n");


            WriteLine("After Adding new Products");

            products = service.GetAllProducts();
            Display(products);

            WriteLine("-----------------------------\n");


            WriteLine("After Updating Products");
            service.UpdateProduct(13, 500);
            service.SaveChanges();
            products = service.GetAllProducts();
            Display(products);
        }
        static void Display(List<Product> productList)
        {
            foreach (Product product in productList)
            {
                WriteLine(product);
            }
        }
    }
}