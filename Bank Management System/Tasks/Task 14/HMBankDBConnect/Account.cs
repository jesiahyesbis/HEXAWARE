using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{

    public abstract class Account
    {
        private static long lastAccNo = 1000000000;

        public long AccountNumber { get; set; }
        public string AccountType { get; protected set; }
        public decimal Balance { get; protected set; }
        public Customer Customer { get; set; }

        protected Account(string accountType, decimal balance, Customer customer)
        {
            AccountNumber = ++lastAccNo;
            AccountType = accountType;
            Balance = balance;
            Customer = customer;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive");
            Balance += amount;
        }

        public abstract void Withdraw(decimal amount);

        public override string ToString()
        {
            return $"Account No: {AccountNumber}, Type: {AccountType}, Balance: {Balance:C}, Customer: {Customer?.Name}";
        }
    }

}
