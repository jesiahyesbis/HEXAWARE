using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.entity;
using OrderManagementSystem.exception;
using OrderManagementSystem.util;



using System.Data.SqlClient;

namespace OrderManagementSystem.dao
{

 
        public class OrderProcessor : IOrderManagementRepository
        {
            public void CreateOrder(User user, List<Product> products)
            {
                SqlConnection connection = null;
                SqlTransaction transaction = null;

                try
                {
                    connection = DBConnUtil.GetDBConn();
                    transaction = connection.BeginTransaction();

                    // Check if user exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE userId = @UserId";
                    SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, connection, transaction);
                    checkUserCmd.Parameters.AddWithValue("@UserId", user.UserId);
                    int userCount = (int)checkUserCmd.ExecuteScalar();

                    if (userCount == 0)
                    {
                        CreateUser(user);
                    }

                    // Create order
                    string orderQuery = "INSERT INTO Orders (userId, orderDate) VALUES (@UserId, @OrderDate); SELECT SCOPE_IDENTITY();";
                    SqlCommand orderCmd = new SqlCommand(orderQuery, connection, transaction);
                    orderCmd.Parameters.AddWithValue("@UserId", user.UserId);
                    orderCmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    int orderId = Convert.ToInt32(orderCmd.ExecuteScalar());

                    // Add order items
                    foreach (var product in products)
                    {
                        string orderItemQuery = "INSERT INTO OrderItems (orderId, productId) VALUES (@OrderId, @ProductId)";
                        SqlCommand orderItemCmd = new SqlCommand(orderItemQuery, connection, transaction);
                        orderItemCmd.Parameters.AddWithValue("@OrderId", orderId);
                        orderItemCmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                        orderItemCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    Console.WriteLine("Order created successfully!");
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    throw new Exception("Error creating order: " + ex.Message);
                }
                finally
                {
                    DBConnUtil.CloseConnection(connection);
                }
            }

            public void CancelOrder(int userId, int orderId)
            {
                SqlConnection connection = null;
                SqlTransaction transaction = null;

                try
                {
                    connection = DBConnUtil.GetDBConn();
                    transaction = connection.BeginTransaction();

                    // Check if user exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE userId = @UserId";
                    SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, connection, transaction);
                    checkUserCmd.Parameters.AddWithValue("@UserId", userId);
                    int userCount = (int)checkUserCmd.ExecuteScalar();

                    if (userCount == 0)
                    {
                        throw new UserNotFoundException($"User with ID {userId} not found.");
                    }

                    // Check if order exists and belongs to user
                    string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE orderId = @OrderId AND userId = @UserId";
                    SqlCommand checkOrderCmd = new SqlCommand(checkOrderQuery, connection, transaction);
                    checkOrderCmd.Parameters.AddWithValue("@OrderId", orderId);
                    checkOrderCmd.Parameters.AddWithValue("@UserId", userId);
                    int orderCount = (int)checkOrderCmd.ExecuteScalar();

                    if (orderCount == 0)
                    {
                        throw new OrderNotFoundException($"Order with ID {orderId} not found for user {userId}.");
                    }

                    // Delete order items
                    string deleteItemsQuery = "DELETE FROM OrderItems WHERE orderId = @OrderId";
                    SqlCommand deleteItemsCmd = new SqlCommand(deleteItemsQuery, connection, transaction);
                    deleteItemsCmd.Parameters.AddWithValue("@OrderId", orderId);
                    deleteItemsCmd.ExecuteNonQuery();

                    // Delete order
                    string deleteOrderQuery = "DELETE FROM Orders WHERE orderId = @OrderId";
                    SqlCommand deleteOrderCmd = new SqlCommand(deleteOrderQuery, connection, transaction);
                    deleteOrderCmd.Parameters.AddWithValue("@OrderId", orderId);
                    deleteOrderCmd.ExecuteNonQuery();

                    transaction.Commit();
                    Console.WriteLine("Order canceled successfully!");
                }
                catch (UserNotFoundException)
                {
                    transaction?.Rollback();
                    throw;
                }
                catch (OrderNotFoundException)
                {
                    transaction?.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    throw new Exception("Error canceling order: " + ex.Message);
                }
                finally
                {
                    DBConnUtil.CloseConnection(connection);
                }
            }

            public void CreateProduct(User user, Product product)
            {
                SqlConnection connection = null;
                SqlTransaction transaction = null;

                try
                {
                    connection = DBConnUtil.GetDBConn();
                    transaction = connection.BeginTransaction();

                    // Check if user is admin
                    string checkAdminQuery = "SELECT COUNT(*) FROM Users WHERE userId = @UserId AND role = 'Admin'";
                    SqlCommand checkAdminCmd = new SqlCommand(checkAdminQuery, connection, transaction);
                    checkAdminCmd.Parameters.AddWithValue("@UserId", user.UserId);
                    int adminCount = (int)checkAdminCmd.ExecuteScalar();

                    if (adminCount == 0)
                    {
                        throw new UnauthorizedAccessException("Only admin users can create products.");
                    }

                    // Insert product
                    string productQuery = "INSERT INTO Products (productId, productName, description, price, quantityInStock, type) " +
                                        "VALUES (@ProductId, @ProductName, @Description, @Price, @QuantityInStock, @Type)";
                    SqlCommand productCmd = new SqlCommand(productQuery, connection, transaction);
                    productCmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    productCmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    productCmd.Parameters.AddWithValue("@Description", product.Description);
                    productCmd.Parameters.AddWithValue("@Price", product.Price);
                    productCmd.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                    productCmd.Parameters.AddWithValue("@Type", product.Type);
                    productCmd.ExecuteNonQuery();

                    // Insert specific product details
                    if (product is Electronics electronics)
                    {
                        string electronicsQuery = "INSERT INTO Electronics (productId, brand, warrantyPeriod) " +
                                               "VALUES (@ProductId, @Brand, @WarrantyPeriod)";
                        SqlCommand electronicsCmd = new SqlCommand(electronicsQuery, connection, transaction);
                        electronicsCmd.Parameters.AddWithValue("@ProductId", electronics.ProductId);
                        electronicsCmd.Parameters.AddWithValue("@Brand", electronics.Brand);
                        electronicsCmd.Parameters.AddWithValue("@WarrantyPeriod", electronics.WarrantyPeriod);
                        electronicsCmd.ExecuteNonQuery();
                    }
                    else if (product is Clothing clothing)
                    {
                        string clothingQuery = "INSERT INTO Clothing (productId, size, color) " +
                                            "VALUES (@ProductId, @Size, @Color)";
                        SqlCommand clothingCmd = new SqlCommand(clothingQuery, connection, transaction);
                        clothingCmd.Parameters.AddWithValue("@ProductId", clothing.ProductId);
                        clothingCmd.Parameters.AddWithValue("@Size", clothing.Size);
                        clothingCmd.Parameters.AddWithValue("@Color", clothing.Color);
                        clothingCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    Console.WriteLine("Product created successfully!");
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    throw new Exception("Error creating product: " + ex.Message);
                }
                finally
                {
                    DBConnUtil.CloseConnection(connection);
                }
            }

            public void CreateUser(User user)
            {
                SqlConnection connection = null;

                try
                {
                    connection = DBConnUtil.GetDBConn();
                    string query = "INSERT INTO Users (userId, username, password, role) " +
                                  "VALUES (@UserId, @Username, @Password, @Role)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("User created successfully!");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating user: " + ex.Message);
                }
                finally
                {
                    DBConnUtil.CloseConnection(connection);
                }
            }

            public List<Product> GetAllProducts()
            {
                List<Product> products = new List<Product>();
                SqlConnection connection = null;
                SqlDataReader reader = null;

                try
                {
                    connection = DBConnUtil.GetDBConn();
                    string query = "SELECT p.*, e.brand, e.warrantyPeriod, c.size, c.color " +
                                 "FROM Products p " +
                                 "LEFT JOIN Electronics e ON p.productId = e.productId " +
                                 "LEFT JOIN Clothing c ON p.productId = c.productId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product;
                        int productId = reader.GetInt32(reader.GetOrdinal("productId"));
                        string type = reader.GetString(reader.GetOrdinal("type"));

                        if (type == "Electronics" && !reader.IsDBNull(reader.GetOrdinal("brand")))
                        {
                            product = new Electronics(
                                productId,
                                reader.GetString(reader.GetOrdinal("productName")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("quantityInStock")),
                                reader.GetString(reader.GetOrdinal("brand")),
                                reader.GetInt32(reader.GetOrdinal("warrantyPeriod"))
                            );
                        }
                        else if (type == "Clothing" && !reader.IsDBNull(reader.GetOrdinal("size")))
                        {
                            product = new Clothing(
                                productId,
                                reader.GetString(reader.GetOrdinal("productName")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("quantityInStock")),
                                reader.GetString(reader.GetOrdinal("size")),
                                reader.GetString(reader.GetOrdinal("color"))
                            );
                        }
                        else
                        {
                            product = new Product(
                                productId,
                                reader.GetString(reader.GetOrdinal("productName")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("quantityInStock")),
                                type
                            );
                        }
                        products.Add(product);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving products: " + ex.Message);
                }
                finally
                {
                    reader?.Close();
                    DBConnUtil.CloseConnection(connection);
                }
                return products;
            }

            public List<Product> GetOrderByUser(User user)
            {
                List<Product> products = new List<Product>();
                SqlConnection connection = null;
                SqlDataReader reader = null;

                try
                {
                    connection = DBConnUtil.GetDBConn();

                    // Check if user exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE userId = @UserId";
                    SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, connection);
                    checkUserCmd.Parameters.AddWithValue("@UserId", user.UserId);
                    int userCount = (int)checkUserCmd.ExecuteScalar();

                    if (userCount == 0)
                    {
                        throw new UserNotFoundException($"User with ID {user.UserId} not found.");
                    }

                    string query = "SELECT p.*, e.brand, e.warrantyPeriod, c.size, c.color " +
                                 "FROM Products p " +
                                 "LEFT JOIN Electronics e ON p.productId = e.productId " +
                                 "LEFT JOIN Clothing c ON p.productId = c.productId " +
                                 "JOIN OrderItems oi ON p.productId = oi.productId " +
                                 "JOIN Orders o ON oi.orderId = o.orderId " +
                                 "WHERE o.userId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product;
                        int productId = reader.GetInt32(reader.GetOrdinal("productId"));
                        string type = reader.GetString(reader.GetOrdinal("type"));

                        if (type == "Electronics" && !reader.IsDBNull(reader.GetOrdinal("brand")))
                        {
                            product = new Electronics(
                                productId,
                                reader.GetString(reader.GetOrdinal("productName")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("quantityInStock")),
                                reader.GetString(reader.GetOrdinal("brand")),
                                reader.GetInt32(reader.GetOrdinal("warrantyPeriod"))
                            );
                        }
                        else if (type == "Clothing" && !reader.IsDBNull(reader.GetOrdinal("size")))
                        {
                            product = new Clothing(
                                productId,
                                reader.GetString(reader.GetOrdinal("productName")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("quantityInStock")),
                                reader.GetString(reader.GetOrdinal("size")),
                                reader.GetString(reader.GetOrdinal("color"))
                            );
                        }
                        else
                        {
                            product = new Product(
                                productId,
                                reader.GetString(reader.GetOrdinal("productName")),
                                reader.GetString(reader.GetOrdinal("description")),
                                reader.GetDecimal(reader.GetOrdinal("price")),
                                reader.GetInt32(reader.GetOrdinal("quantityInStock")),
                                type
                            );
                        }
                        products.Add(product);
                    }
                }
                catch (UserNotFoundException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving user orders: " + ex.Message);
                }
                finally
                {
                    reader?.Close();
                    DBConnUtil.CloseConnection(connection);
                }
                return products;
            }
        }
    
}
