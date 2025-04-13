using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
namespace AdoConnectedDemo
{
    internal class UserInterface
    {
        public int GetProductId() {
            Console.WriteLine("Enter Product ID");
            return ToInt32(Console.ReadLine());
        }
        public string GetProductName()
        {

            Console.WriteLine("Enter Product Name");
            return Console.ReadLine();
        }

        public decimal GetProductPrice()
        {

            Console.WriteLine("Enter Product Price");
            return ToDecimal(Console.ReadLine());
        }

        public string GetProductCategory()
        {

            Console.WriteLine("Enter Product Category");
            return Console.ReadLine();
        }

        public decimal GetLowPrice()
        {
            Console.WriteLine("Enter Low Price:");
            return Convert.ToDecimal(Console.ReadLine());
        }

        public decimal GetHighPrice()
        {
            Console.WriteLine("Enter High Price:");
            return Convert.ToDecimal(Console.ReadLine());
        }

    }
}
