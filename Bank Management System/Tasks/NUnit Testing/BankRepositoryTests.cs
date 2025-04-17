using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMBankDBConnect;

namespace BankingTestProject1
{
    [TestFixture]
    public class BankRepositoryTests
    {
        private BankRepositoryImpl _repository;
        private SqlConnection _testConnection;

        [SetUp]
        public void Setup()
        {
            _testConnection = DBUtil.GetDBConn();
            _repository = new BankRepositoryImpl();
        }

        [TearDown]
        public void Cleanup()
        {
            _testConnection?.Close();
        }

        [Test]
        public void Deposit_ValidAccount_UpdatesBalance()
        {
            // Arrange
            long testAccountNumber = 1001; // Use existing test account
            decimal initialBalance = _repository.GetAccountBalance(testAccountNumber);
            decimal depositAmount = 500m;

            // Act
            decimal newBalance = _repository.Deposit(testAccountNumber, depositAmount);

            // Assert
            Assert.That(newBalance, Is.EqualTo(initialBalance + depositAmount));
        }
    }
}
