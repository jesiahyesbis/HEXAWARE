using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class BankServiceProviderImpl : CustomerServiceProviderImpl, IBankServiceProvider
    {
        private List<Account> accountList;
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }

    public BankServiceProviderImpl(string branchName, string branchAddress)
    {
        BranchName = branchName;
        BranchAddress = branchAddress;
        accountList = new List<Account>();
    }

    public void CreateAccount(Customer customer, long accNo, string accType, double balance)
    {
        Account newAccount = new Account(accNo, accType, balance, customer);
        accountList.Add(newAccount);
        Console.WriteLine($"Account {accNo} created successfully.");
    }

    public List<Account> ListAccounts()
    {
        return accountList;
    }

    public void CalculateInterest(long accountNumber)
    {
        var account = accountList.Find(a => a.AccountNumber == accountNumber);
        if (account != null && account.AccountType == "Savings")
        {
            double interest = account.AccountBalance * 0.045; // 4.5% interest rate
            account.AccountBalance += interest;
            Console.WriteLine($"Interest added: {interest}. New balance: {account.AccountBalance}");
        }
        else
        {
            Console.WriteLine("Interest calculation is only available for savings accounts.");
        }
    }
    }
}
