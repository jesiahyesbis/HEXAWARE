using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.dao;
using OrderManagementSystem.entity;
using OrderManagementSystem.exception;

namespace OrderManagementSystem.main
{
 
        public class MainModule
        {
            private static IOrderManagementRepository repository = new OrderProcessor();

            public static void Main(string[] args)
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nOrder Management System");
                    Console.WriteLine("1. Create User");
                    Console.WriteLine("2. Create Product (Admin only)");
                    Console.WriteLine("3. Create Order");
                    Console.WriteLine("4. Cancel Order");
                    Console.WriteLine("5. Get All Products");
                    Console.WriteLine("6. Get Orders by User");
                    Console.WriteLine("7. Exit");
                    Console.Write("Enter your choice: ");

                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                CreateUser();
                                break;
                            case 2:
                                CreateProduct();
                                break;
                            case 3:
                                CreateOrder();
                                break;
                            case 4:
                                CancelOrder();
                                break;
                            case 5:
                                GetAllProducts();
                                break;
                            case 6:
                                GetOrdersByUser();
                                break;
                            case 7:
                                exit = true;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (UserNotFoundException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (OrderNotFoundException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            private static void CreateUser()
            {
                Console.WriteLine("\nCreate New User");
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();
                Console.Write("Enter Role (Admin/User): ");
                string role = Console.ReadLine();

                User user = new User(userId, username, password, role);
                repository.CreateUser(user);
            }

            private static void CreateProduct()
            {
                Console.WriteLine("\nCreate New Product (Admin only)");
                Console.Write("Enter Admin User ID: ");
                int adminId = int.Parse(Console.ReadLine());

                Console.Write("Enter Product ID: ");
                int productId = int.Parse(Console.ReadLine());
                Console.Write("Enter Product Name: ");
                string productName = Console.ReadLine();
                Console.Write("Enter Description: ");
                string description = Console.ReadLine();
                Console.Write("Enter Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
                Console.Write("Enter Quantity in Stock: ");
                int quantity = int.Parse(Console.ReadLine());
                Console.Write("Enter Type (Electronics/Clothing): ");
                string type = Console.ReadLine();

                Product product;
                if (type.Equals("Electronics", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Brand: ");
                    string brand = Console.ReadLine();
                    Console.Write("Enter Warranty Period (months): ");
                    int warranty = int.Parse(Console.ReadLine());
                    product = new Electronics(productId, productName, description, price, quantity, brand, warranty);
                }
                else if (type.Equals("Clothing", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("Enter Size: ");
                    string size = Console.ReadLine();
                    Console.Write("Enter Color: ");
                    string color = Console.ReadLine();
                    product = new Clothing(productId, productName, description, price, quantity, size, color);
                }
                else
                {
                    product = new Product(productId, productName, description, price, quantity, type);
                }

                User admin = new User(adminId, "", "", "Admin");
                repository.CreateProduct(admin, product);
            }

            private static void CreateOrder()
            {
                Console.WriteLine("\nCreate New Order");
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();
                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                User user = new User(userId, username, password, "User");

                List<Product> products = new List<Product>();
                bool addMore = true;
                while (addMore)
                {
                    Console.Write("Enter Product ID to order: ");
                    int productId = int.Parse(Console.ReadLine());

                    Product product = new Product();
                    product.ProductId = productId;
                    products.Add(product);

                    Console.Write("Add another product? (y/n): ");
                    addMore = Console.ReadLine().ToLower() == "y";
                }

                repository.CreateOrder(user, products);
            }

            private static void CancelOrder()
            {
                Console.WriteLine("\nCancel Order");
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());
                Console.Write("Enter Order ID to cancel: ");
                int orderId = int.Parse(Console.ReadLine());

                repository.CancelOrder(userId, orderId);
            }

            private static void GetAllProducts()
            {
                Console.WriteLine("\nAll Products:");
                List<Product> products = repository.GetAllProducts();
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Type: {product.Type}, Price: {product.Price}");
                    if (product is Electronics electronics)
                    {
                        Console.WriteLine($"  Brand: {electronics.Brand}, Warranty: {electronics.WarrantyPeriod} months");
                    }
                    else if (product is Clothing clothing)
                    {
                        Console.WriteLine($"  Size: {clothing.Size}, Color: {clothing.Color}");
                    }
                }
            }

            private static void GetOrdersByUser()
            {
                Console.WriteLine("\nGet Orders by User");
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();

                User user = new User(userId, username, "", "User");
                List<Product> products = repository.GetOrderByUser(user);

                Console.WriteLine($"Orders for User {username}:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Type: {product.Type}, Price: {product.Price}");
                }
            }
        }
    
}
