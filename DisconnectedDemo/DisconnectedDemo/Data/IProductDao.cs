using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDemo.Models;

namespace DisconnectedDemo.Data
{
    public interface IProductDao
    {
        DataTable GetAllProducts();
        void AddProduct(Product product);

       void UpdateProduct(int id, decimal price);

        void DeleteProduct(int id);
    }
}
