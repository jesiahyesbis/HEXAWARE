using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMBankDBConnect;

namespace BankingTestProject1
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void EmailValidation_InvalidEmail_ThrowsArgumentException()
        {
            var customer = new Customer();
            Assert.Throws<ArgumentException>(() =>
            {
                customer.EmailAddress = "invalid-email";
            });
        }
    }
}
