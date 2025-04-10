using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; }

        public SavingsAccount(decimal balance, decimal interestRate, Customer customer)
            : base("Savings", Math.Max(balance, 500), customer) // Minimum balance 500
        {
            InterestRate = interestRate;
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            if (Balance - amount < 500)
                throw new InvalidOperationException("Minimum balance of 500 must be maintained");

            Balance -= amount;
        }

        public override string ToString()
        {
            return base.ToString() + $", Interest Rate: {InterestRate:P}";
        }
    }

}
