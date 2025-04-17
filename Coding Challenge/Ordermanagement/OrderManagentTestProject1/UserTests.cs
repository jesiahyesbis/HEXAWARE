using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.entity;

namespace OrderManagementTestProject
{
 
        [TestFixture]
        public class UserTests
        {
            [Test]
            public void User_Constructor_SetsProperties()
            {
                // Arrange //Act
                var user = new User(1, "john_doe", "password123", "Admin");

                // Assert
                Assert.That(user.UserId, Is.EqualTo(1));
                Assert.That(user.Username, Is.EqualTo("john_doe"));
                Assert.That(user.Password, Is.EqualTo("password123"));
                Assert.That(user.Role, Is.EqualTo("Admin"));
            }
        }
    
}
