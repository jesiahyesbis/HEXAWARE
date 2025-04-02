using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class CurrentAccount : Account
    {
        // public const double OverdraftLimit = 1000.00;
        public double OverdraftLimit { get; set; }

        // Constructor for CurrentAccount
        public CurrentAccount(long accountNumber, double balance, double overdraftLimit)
            : base(accountNumber, "Current", balance)
        {
            OverdraftLimit = overdraftLimit;
        }

        // Constructor
        public CurrentAccount(long accountNumber, string accountType, double accountBalance)
            : base(accountNumber, accountType, accountBalance)
        {
        }

        // Override withdraw method for overdraft
        public new void Withdraw(double amount)
        {
            if (AccountBalance + OverdraftLimit >= amount)
            {
                AccountBalance -= amount;
                Console.WriteLine($"Withdrawn {amount}. New balance is: {AccountBalance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance and overdraft limit reached!");
            }
        }
    }
}
