using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class Account
    {
        long accountNumber;
        string accountType;
        double accountBalance;
        private const double interestRate = 0.045;

        public long AccountNumber {
            get { return accountNumber; }
            set { accountNumber = value; }
        }
        public string AccountType {
            get { return accountType; }
            set { accountType = value; }
        }
        public double AccountBalance {
            get { return accountBalance; }
            set { accountBalance = value; }
        }

        public Customer AccountHolder { get; set; }

        // Default constructor
        public Account()
        {
        }

        // Overloaded constructor
        public Account(long accountNumber, string accountType, double accountBalance)
        {
            this.accountNumber = accountNumber;
            this.accountType = accountType;
            this.accountBalance = accountBalance;
        }


        public Account(long accountNumber, string accountType, double accountBalance, Customer accountHolder)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            AccountBalance = accountBalance;
            AccountHolder = accountHolder;
        }

        // Deposit method
        public virtual void Deposit(double amount)
        {
            AccountBalance += amount;
            Console.WriteLine($"Deposited {amount}. New balance is: {AccountBalance}");
        }

        // Withdraw method
        public virtual void Withdraw(double amount)
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount;
                Console.WriteLine($"Withdrawn {amount}. New balance is: {AccountBalance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance!");
            }
        }

        // Calculate interest method
        public virtual void CalculateInterest()
        {
            double interest = AccountBalance * interestRate;
            Console.WriteLine($"Interest calculated at {interestRate * 100}%: {interest}");
            AccountBalance += interest;  // Add interest to account balance
            Console.WriteLine($"New balance after interest: {AccountBalance}");
        }


        public void Deposit(int amount)
        {
            AccountBalance += amount;
            Console.WriteLine($"Deposited {amount}. New balance is: {AccountBalance}");
        }

        public void Withdraw(int amount)
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount;
                Console.WriteLine($"Withdrawn {amount}. New balance is: {AccountBalance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance!");
            }
        }

        // Deposit method (accepts float)
        public void Deposit(float amount)
        {
            AccountBalance += amount;
            Console.WriteLine($"Deposited {amount}. New balance is: {AccountBalance}");
        }

        // Withdraw method (accepts float)
        public void Withdraw(float amount)
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount;
                Console.WriteLine($"Withdrawn {amount}. New balance is: {AccountBalance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance!");
            }
        }
        // Print account details
        public void DisplayAccountDetails()
        {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Type: {AccountType}");
            Console.WriteLine($"Account Balance: {AccountBalance}");
        }
    }
}
