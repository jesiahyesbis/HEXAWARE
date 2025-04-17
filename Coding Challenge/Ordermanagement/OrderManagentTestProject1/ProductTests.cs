using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.entity;

namespace OrderManagementTestProject
{
    [TestFixture]

    public class ProductTests
    {
     

            [Test]
            public void Product_DefaultConstructor_InitializesProperties()
            {
                // Arrange // Act
                var product = new Product();

                // Assert
                Assert.That(product.ProductId, Is.EqualTo(0));
                Assert.That(product.ProductName, Is.Null);
                Assert.That(product.Description, Is.Null);
                Assert.That(product.Price, Is.EqualTo(0));
                Assert.That(product.QuantityInStock, Is.EqualTo(0));
                Assert.That(product.Type, Is.Null);
            }
    }
}


