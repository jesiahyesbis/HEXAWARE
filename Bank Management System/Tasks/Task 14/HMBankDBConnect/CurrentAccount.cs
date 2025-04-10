using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class CurrentAccount : Account
    {
        public decimal OverdraftLimit { get; }

        public CurrentAccount(decimal balance, decimal overdraftLimit, Customer customer)
            : base("Current", balance, customer)
        {
            OverdraftLimit = overdraftLimit;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            if (Balance + OverdraftLimit < amount)
                throw new InvalidOperationException("Withdrawal exceeds available balance and overdraft limit");

            Balance -= amount;
        }

        public override string ToString()
        {
            return base.ToString() + $", Overdraft Limit: {OverdraftLimit:C}";
        }
    }

}
