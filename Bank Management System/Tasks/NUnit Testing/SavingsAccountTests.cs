using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMBankDBConnect;

namespace BankingTestProject1
{
    [TestFixture]
    public class SavingsAccountTests
    {
        [Test]
        public void Constructor_BalanceBelow500_ThrowsArgumentException()
        {
            var customer = new Customer(1, "John", "Doe", "john@test.com", "1234567890", "123 Main St");
            Assert.That(() => new SavingsAccount(customer, 499, 4.0m),
                Throws.ArgumentException.With.Message.Contains("minimum balance"));
        }
    }
}
