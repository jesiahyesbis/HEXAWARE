using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    public abstract class BankAccount
    {
        // Properties
        public long AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public double Balance { get; set; }

        // Default constructor
        public BankAccount() { }

        // Overloaded constructor
        public BankAccount(long accountNumber, string customerName, double balance)
        {
            AccountNumber = accountNumber;
            CustomerName = customerName;
            Balance = balance;
        }

        // Abstract methods (to be implemented by derived classes)
        public abstract void Deposit(double amount);
        public abstract void Withdraw(double amount);
        public abstract void CalculateInterest();
    }
}
