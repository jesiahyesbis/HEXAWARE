using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoConnectedDemo.Models
{
    internal class ProductException:Exception
    {
        public ProductException() : base() {}
        public ProductException(string message) : base(message) {}

    }
}
