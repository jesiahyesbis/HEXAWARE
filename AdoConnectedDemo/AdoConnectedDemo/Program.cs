using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdoConnectedDemo.Data;
using AdoConnectedDemo.Models;
using static System.Console;

namespace AdoConnectedDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Disconnected
            string query = "select * from products";

            string connectionString = @"Server = DESKTOP-EHHLEIA\SQLEXPRESS ; Database = hexaware ; Integrated Security=True ; MultipleActiveResultSets=true;";



            //Open connection
            try
            {
               // SqlConnection sqlConnection = DBUtility.GetConnection();

                SqlConnection sqlConnection = new SqlConnection(connectionString);



                //instance for adapter
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                //Create dataset
                DataSet dataSet = new DataSet();

                //fill the dataset
                adapter.Fill(dataSet, "products");

                DataTable productsTable = dataSet.Tables["products"];


                foreach (DataRow row in productsTable.Rows)
                {
                    WriteLine($"Id : {row[0]}");
                    WriteLine($"Name : {row[1]}");
                    WriteLine($"Price : {row[2]}");
                    WriteLine($"Category : {row[3]}");
                    WriteLine("------------------------------------");

                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);


            }



            //Connected
            //UserInterface ui = new UserInterface();
            //ProductDaoImpl productDao = new ProductDaoImpl();
            ////List<Product> productList = productDao.GetAllProducts();
            ////Display(productList);
            //try
            //{
            //    //UserInterface ui = new UserInterface();
            //    //Product p = new Product();
            //    //p.ProductId = ui.GetProductId();
            //    //p.ProductName = ui.GetProductName();
            //    //p.Price = ui.GetProductPrice();
            //    //p.Category = ui.GetProductCategory();
            //    //productDao.AddProduct(p);

            //    //UpdateProductPrice
            //    //int id = ui.GetProductId();
            //    //decimal price = ui.GetProductPrice();
            //    //int result=productDao.UpdateProductPrice(id, price);
            //    //Console.WriteLine("The updated product "+productDao.GetProductById(id));


            //    //DeleteProduct
            //    //Console.WriteLine("Enter the id to be deleted");
            //    //int id = ui.GetProductId();
            //    //int result = productDao.DeleteProduct(id);
            //    //Console.WriteLine($"Deleted Product with ID: {id}, Rows Affected: {result}");


            //    //SortByLowPriceFirst
            //    Console.WriteLine("Sort Products by Low Price First");
            //    List<Product> sortedByPrice = productDao.SortByLowPriceFirst();
            //    Display(sortedByPrice);

            //    //GetProductsByCategory
            //    Console.WriteLine("Get Products by Category");
            //    string category = ui.GetProductCategory();
            //    List<Product> productByCategory = productDao.GetProductsByCategory(category);
            //    Display(productByCategory);

            //    //GetProductsByPriceRange
            //    Console.WriteLine("Get Products by Price Range");
            //    decimal low = ui.GetLowPrice();
            //    decimal high = ui.GetHighPrice();
            //    List<Product> productsByPriceRange = productDao.GetProductsByPriceRange(low, high);
            //    Display(productsByPriceRange);

            //    //Console.WriteLine("---------------Product List-----------------");
            //    //List<Product> productList = productDao.GetAllProducts();
            //    //Display(productList);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

        }

        static void Display(List<Product> productList)
        {
            foreach (Product product in productList)
            {
                Console.WriteLine(product);
            }
        }
    }
}