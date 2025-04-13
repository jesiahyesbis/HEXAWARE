using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnectedDemo.Models;

namespace AdoConnectedDemo.Data
{
    internal interface IProductDao
    {
        int AddProduct(Product product);
        //int UpdateProduct(Product product);

        int UpdateProductPrice(int id, decimal newPrice);

        int DeleteProduct(int id);
        Product GetProductByName(string name);
        Product GetProductById(int id);
        List<Product> GetAllProducts();

        List<Product> SortByLowPriceFirst();
        List<Product> GetProductsByCategory(string category);

        List<Product> GetProductsByPriceRange(decimal low, decimal high);

        //Case insensitive and Part of string
        //public List<Product> GetProductByName(string name)
    }
}
