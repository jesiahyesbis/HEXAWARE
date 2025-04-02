using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class ZeroBalanceAccount : Account
    {
        public ZeroBalanceAccount(long accountNumber)
        : base(accountNumber, "ZeroBalance", 0)
        {
        }

        // Overriding the Withdraw method since the balance is always zero
        public override void Withdraw(double amount)
        {
            Console.WriteLine("Withdraw cannot be performed. Account balance is zero.");
        }

        // Optionally, we could also add a Deposit method to allow adding funds to a ZeroBalance account.
        public override void Deposit(double amount)
        {
            base.Deposit(amount);
        }
    }
}
