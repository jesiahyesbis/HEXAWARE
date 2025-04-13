using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnectedDemo.Models;

namespace AdoConnectedDemo.Data
{
    internal class ProductDaoImpl:IProductDao
    {
     
        public int AddProduct(Product product) {
            SqlConnection con = null;
            SqlCommand command = null;
            int rowsAffected = 0;

            string query = $"insert into products(product_id,product_name,price,category) values(@pid,@pname,@price,@pcategory);";

            try
            {

                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@pname", product.ProductName));


                    command.Parameters.Add(new SqlParameter("@price", product.Price));


                    command.Parameters.Add(new SqlParameter("@pcategory", product.Category));

                    command.Parameters.Add(new SqlParameter("@pid", product.ProductId));

                    rowsAffected = command.ExecuteNonQuery();

                }

            }

            catch (SqlException ex)
            {
                throw ex;
            }

            catch (Exception ex)
            {
                throw new Exception("Error in adding a new product");
            }
            return rowsAffected;

        }

        


       

        
        public List<Product> GetAllProducts()
        {
            List<Product> products=new List<Product>();
            Product product=null;
            SqlConnection con=null;
            SqlCommand command=null;
            //sql qyery to retrieve all products
            string query="select * from products";
            try
            {
                //open the conection
                using(con=DBUtility.GetConnection())
                {

                    //create command object the sql query &conection
                    command=new SqlCommand(query,con);
                    //Execute command
                    SqlDataReader reader=command.ExecuteReader();

                    while(reader.Read())
                        {
                            product=new Product();
                            product.ProductId=reader.GetInt32(0);
                            product.ProductName=reader.GetString(1);
                            product.Price=reader.GetDecimal(2);
                            product.Category=reader.GetString(3);
                            products.Add(product);
                        }
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }


            return products;
         }





       

        public Product GetProductById(int id)
        {
            Product product = null;
            SqlConnection con = null;
            SqlCommand command= null;
            string query = "Select * from products where product_id=@id";

            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        product = new Product();
                        product.ProductId = (int)reader["product_id"];
                        product.ProductName = (string)reader["product_name"];
                        product.Price = (decimal)reader["price"];
                        product.Category = (string)reader["category"];

                    }
                }

                if (product == null)
                {
                    throw new ProductException("Id not found");
                }

            }

            catch (SqlException ex)
            {

                throw ex;
            }

            catch (Exception ex)
            {

                throw new Exception("error fetching Product with the given id: " + ex.Message);
            }

            return product;

        }

        

        public Product GetProductByName(string name)
        {
            Product product = null;
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "SELECT * FROM products WHERE product_name = @name";

            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@name", name);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        product = new Product();
                        product.ProductId = (int)reader["product_id"];
                        product.ProductName = (string)reader["product_name"];
                        product.Price = (decimal)reader["price"];
                        product.Category = (string)reader["category"];
                    }
                }

                if (product == null)
                {
                    throw new ProductException("Product name not found");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Product with the given name: " + ex.Message);
            }

            return product;

        }


        public int UpdateProductPrice(int id, decimal newPrice)
        {
            SqlConnection con = null;
            SqlCommand command = null;

            int rowsAffected = 0;
            Product pdt = GetProductById(id);
            if (pdt == null)
            {
                throw new ProductException($"Product nnot found for the given {id}");

            }
            else
            {
                string query = "update products set price= @price where product_id=@pid";
                try
                {
                    using (con = DBUtility.GetConnection())
                    {
                        command = new SqlCommand(query, con);
                        command.Parameters.AddWithValue("@pid", id);
                        command.Parameters.AddWithValue("@price", newPrice);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

            }

            return rowsAffected;
        }




        public int DeleteProduct(int id)
        {
            SqlConnection con = null;
            SqlCommand command = null;

            int rowsAffected = 0;
            string query = "delete from products where product_id=@id";
            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        throw new ProductException("Id not found, Couldn't delete Product");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return rowsAffected;

        }




        public List<Product> SortByLowPriceFirst()
        {
            List<Product> products = new List<Product>();
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "SELECT * FROM products ORDER BY price ASC";

            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Category = reader.GetString(3)
                        };
                        products.Add(product);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return products;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            List<Product> products = new List<Product>();
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "SELECT * FROM products WHERE category = @category";

            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@category", category);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Category = reader.GetString(3)
                        };
                        products.Add(product);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return products;
        }

        public List<Product> GetProductsByPriceRange(decimal low, decimal high)
        {
            List<Product> products = new List<Product>();
            SqlConnection con = null;
            SqlCommand command = null;
            string query = "SELECT * FROM products WHERE price BETWEEN @low AND @high";

            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@low", low);
                    command.Parameters.AddWithValue("@high", high);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            Price = reader.GetDecimal(2),
                            Category = reader.GetString(3)
                        };
                        products.Add(product);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return products;
        }




        /*
         public List<Product> GetProductByName(string name)
{
    List<Product> products = new List<Product>();
    SqlConnection con = null;
    SqlCommand command = null;
    string query = "SELECT * FROM products WHERE LOWER(product_name) LIKE @name";

    try
    {
        using (con = DBUtility.GetConnection())
        {
            command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@name", "%" + name.ToLower() + "%");

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = (int)reader["product_id"],
                    ProductName = (string)reader["product_name"],
                    Price = (decimal)reader["price"],
                    Category = (string)reader["category"]
                };
                products.Add(product);
            }
        }

        if (products.Count == 0)
        {
            throw new ProductException("No products match the given name.");
        }
    }
    catch (SqlException ex)
    {
        throw ex;
    }
    catch (Exception ex)
    {
        throw new Exception("Error fetching product(s) by name: " + ex.Message);
    }

    return products;
}
         */

    }
}
