using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedDemo.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public Product()
        {

        }

        public Product(int productId,string productName,decimal price,string category)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Category = category;
        }
        public override string ToString()
        {
            return $"{ProductId}{ProductName}{Price}{Category}";
        }

    }
}
