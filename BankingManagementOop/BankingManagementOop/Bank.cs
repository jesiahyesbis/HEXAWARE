using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class Bank
    {
        private static long accountNumberCounter = 1001;
        private List<Account> accounts = new List<Account>();

        //BankApp
        public void CreateAccount(Customer customer, string accountType, double balance)
        {
            Account account = null;

            switch (accountType)
            {
                case "Savings":
                    account = new SavingsAccount(accountNumberCounter++, balance, 4.5);
                    break;
                case "Current":
                    account = new CurrentAccount(accountNumberCounter++, balance, 1000);
                    break;
                default:
                    Console.WriteLine("Invalid account type.");
                    return;
            }

            accounts.Add(account);
            Console.WriteLine($"{accountType} account created successfully with Account Number: {account.AccountNumber}");
        }


        public double GetAccountBalance(long accountNumber)
        {
            Account account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                return account.AccountBalance;
            }
            Console.WriteLine("Account not found.");
            return -1;
        }

        // Deposit method
        public void Deposit(long accountNumber, double amount)
        {
            Account account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.Deposit(amount);
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        // Withdraw method
        public void Withdraw(long accountNumber, double amount)
        {
            Account account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.Withdraw(amount);
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

       

        // Get account details
        public void GetAccountDetails(long accountNumber)
        {
            Account account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.DisplayAccountDetails();
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }




        // Create an account for a customer
        public void CreateAccount(Customer customer, long accountNumber, string accountType, double balance)
        {
            Account account;
            if (accountType == "Savings")
            {
                account = new SavingsAccount(accountNumber, accountType, balance, 4.5);
            }
            else if (accountType == "Current")
            {
                account = new CurrentAccount(accountNumber, accountType, balance);
            }
            else
            {
                Console.WriteLine("Invalid account type.");
                return;
            }

            accounts.Add(account);
            Console.WriteLine("Account created successfully.");
        }

        

        // Transfer money between accounts
        public void Transfer(long fromAccountNumber, long toAccountNumber, double amount)
        {
            var fromAccount = accounts.FirstOrDefault(a => a.AccountNumber == fromAccountNumber);
            var toAccount = accounts.FirstOrDefault(a => a.AccountNumber == toAccountNumber);

            if (fromAccount != null && toAccount != null)
            {
                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
                Console.WriteLine($"Transferred {amount} from account {fromAccountNumber} to {toAccountNumber}");
            }
            else
            {
                Console.WriteLine("Invalid account numbers.");
            }
        }
    }
}
