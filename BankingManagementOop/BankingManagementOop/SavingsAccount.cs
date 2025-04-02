using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class SavingsAccount : Account
    {
        public double InterestRate { get; set; }
        private const double MinimumBalance = 500;


        public SavingsAccount(long accountNumber, double balance, double interestRate)
        : base(accountNumber, "Savings", balance)
        {
            if (balance < MinimumBalance)
            {
                throw new InvalidOperationException("Minimum balance for a Savings Account is 500.");
            }

            InterestRate = interestRate;
        }

        // Constructor
        public SavingsAccount(long accountNumber, string accountType, double accountBalance, double interestRate)
            : base(accountNumber, accountType, accountBalance)
        {
            InterestRate = interestRate;
        }

        // Override calculate_interest
        public override void CalculateInterest()
        {
            double interest = AccountBalance * (InterestRate / 100);
            Console.WriteLine($"Interest: {interest}. Total balance after interest: {AccountBalance + interest}");
        }
    }
}
