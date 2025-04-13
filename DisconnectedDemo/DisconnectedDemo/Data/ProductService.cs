using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDemo.Models;
using System.Data.SqlClient;
using System.Data;


namespace DisconnectedDemo.Data
{
    public class ProductService
    {
        ProductDaoImpl productDao=new ProductDaoImpl();

        public ProductService()
        {
            productDao = new ProductDaoImpl();
        }

        public List<Product> GetAllProducts()
        {

            DataTable dt = productDao.GetAllProducts();
            List<Product> products = new List<Product>();

            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState != System.Data.DataRowState.Deleted)
                {
                    products.Add(new Product
                    {
                        ProductId = (int)row["product_id"],
                        ProductName = row["product_name"].ToString(),
                        Price = (decimal)row["price"],
                        Category = row["category"].ToString()
                    });


                }

            }

            return products;

        }


        public void AddProduct(Product product) => productDao.AddProduct(product);

        public void UpdateProduct(int id, decimal price) => productDao.UpdateProduct(id, price);

        public void DeleteProduct(int id) => productDao.DeleteProduct(id);

        public void SaveChanges() => productDao.SaveChanges();

    }
}
