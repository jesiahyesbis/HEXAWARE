using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HMBankDBConnect
{
    public class BankServiceProviderImpl : CustomerServiceProviderImpl, IBankServiceProvider
    {
        public string BranchName { get; }
        public string BranchAddress { get; }

        public BankServiceProviderImpl(string branchName, string branchAddress)
        {
            BranchName = branchName;
            BranchAddress = branchAddress;
        }

        public void CreateAccount(Customer customer, string accType, decimal balance)
        {
            Account account = accType switch
            {
                "Savings" => new SavingsAccount(balance, 0.04m, customer),
                "Current" => new CurrentAccount(balance, 1000m, customer),
                "ZeroBalance" => new ZeroBalanceAccount(customer),
                _ => throw new ArgumentException("Invalid account type")
            };

            accounts.Add(account);
            Console.WriteLine($"Created {accType} account for {customer.Name}");
        }

        public List<Account> ListAccounts() => accounts;

        public void CalculateInterest()
        {
            foreach (var account in accounts)
            {
                if (account is SavingsAccount savingsAccount)
                {
                    decimal interest = savingsAccount.Balance * savingsAccount.InterestRate / 12;
                    savingsAccount.Deposit(interest);

                    transactions.Add(new Transaction(
                        savingsAccount.AccountNumber,
                        "Interest Credit",
                        DateTime.Now,
                        "Interest",
                        interest
                    ));
                }
            }
        }
    }

}
