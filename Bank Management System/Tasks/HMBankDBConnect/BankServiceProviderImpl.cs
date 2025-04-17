using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace HMBankDBConnect
{
    public class BankServiceProviderImpl : CustomerServiceProviderImpl, IBankServiceProvider
    {
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }

        public BankServiceProviderImpl(string branchName, string branchAddress) : base()
        {
            BranchName = branchName;
            BranchAddress = branchAddress;
        }

        public Account CreateAccount(Customer customer, long accNo, string accType, decimal balance)
        {
            Account newAccount = null;

            switch (accType.ToLower())
            {
                case "savings":
                    newAccount = new SavingsAccount(customer, balance, 4.0m); // Default 4% interest
                    break;
                case "current":
                    newAccount = new CurrentAccount(customer, balance, 10000m); // Default 10,000 overdraft
                    break;
                case "zerobalance":
                    newAccount = new ZeroBalanceAccount(customer);
                    break;
                default:
                    throw new ArgumentException("Invalid account type.");
            }

            accountList.Add(newAccount);
            return newAccount;
        }

        public List<Account> ListAccounts()
        {
            return accountList;
        }

        public void CalculateInterest()
        {
            foreach (var account in accountList)
            {
                if (account is SavingsAccount savingsAccount)
                {
                    decimal interest = savingsAccount.Balance * (savingsAccount.InterestRate / 100);
                    savingsAccount.Balance += interest;
                    transactionList.Add(new Transaction(savingsAccount, "Interest Credit", "Deposit", interest));
                }
            }
        }
    }
}










