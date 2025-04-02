using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class CustomerServiceProviderImpl : ICustomerServiceProvider
    {
        private List<Account> accounts;

        public CustomerServiceProviderImpl()
        {
            accounts = new List<Account>();  // Initialize the accounts list.
        }

        // Method to get the balance of an account
        public double GetAccountBalance(long accountNumber)
        {
            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                return account.AccountBalance;
            }
            else
            {
                Console.WriteLine("Account not found!");
                return 0;
            }
        }

        // Method to deposit an amount into an account
        public double Deposit(long accountNumber, double amount)
        {
            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                account.AccountBalance += amount;
                Console.WriteLine($"Deposited {amount} into account {accountNumber}. New balance is: {account.AccountBalance}");
                return account.AccountBalance;
            }
            else
            {
                Console.WriteLine("Account not found!");
                return 0;
            }
        }

        // Method to withdraw an amount from an account
        public double Withdraw(long accountNumber, double amount)
        {
            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                if (account.AccountBalance >= amount)
                {
                    account.AccountBalance -= amount;
                    Console.WriteLine($"Withdrawn {amount} from account {accountNumber}. New balance is: {account.AccountBalance}");
                    return account.AccountBalance;
                }
                else
                {
                    Console.WriteLine("Insufficient balance!");
                    return account.AccountBalance;
                }
            }
            else
            {
                Console.WriteLine("Account not found!");
                return 0;
            }
        }

        // Method to transfer an amount from one account to another
        public void Transfer(long fromAccountNumber, long toAccountNumber, double amount)
        {
            var fromAccount = accounts.Find(a => a.AccountNumber == fromAccountNumber);
            var toAccount = accounts.Find(a => a.AccountNumber == toAccountNumber);

            if (fromAccount != null && toAccount != null)
            {
                if (fromAccount.AccountBalance >= amount)
                {
                    fromAccount.AccountBalance -= amount;
                    toAccount.AccountBalance += amount;
                    Console.WriteLine($"Transferred {amount} from account {fromAccountNumber} to account {toAccountNumber}");
                }
                else
                {
                    Console.WriteLine("Insufficient balance for transfer!");
                }
            }
            else
            {
                Console.WriteLine("One or both accounts not found!");
            }
        }

        // Method to get the details of an account
        public string GetAccountDetails(long accountNumber)
        {
            var account = accounts.Find(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                return $"Account Number: {account.AccountNumber}, Account Type: {account.AccountType}, Balance: {account.AccountBalance}";
            }
            else
            {
                return "Account not found!";
            }
        }

        // Method to add a new account to the accounts list (can be used to create an account)
        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

    }
}
