using System;
using System.Collections.Generic;

namespace ExceptionHandling
{
    public class InsufficientFundException : Exception
    {
        public InsufficientFundException(string message) : base(message) { }
    }

    public class InvalidAccountException : Exception
    {
        public InvalidAccountException(string message) : base(message) { }
    }

    public class OverDraftLimitExceededException : Exception
    {
        public OverDraftLimitExceededException(string message) : base(message) { }
    }

    public abstract class Account
    {
        public string AccountNumber { get; }
        public string CustomerName { get; }
        public decimal Balance { get; protected set; }

        protected Account(string accountNumber, string customerName, decimal balance)
        {
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
            Balance = balance;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive");
            Balance += amount;
        }

        public abstract void Withdraw(decimal amount);

        public virtual void Transfer(Account destination, decimal amount)
        {
            if (destination == null)
                throw new NullReferenceException("Destination account cannot be null");

            Withdraw(amount);
            destination.Deposit(amount);
        }
    }

    public class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, string customerName, decimal balance)
            : base(accountNumber, customerName, balance) { }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            if (Balance < amount)
                throw new InsufficientFundException($"Insufficient funds in account {AccountNumber}");

            Balance -= amount;
        }
    }

    public class CurrentAccount : Account
    {
        public decimal OverdraftLimit { get; set; } = 10000;

        public CurrentAccount(string accountNumber, string customerName, decimal balance)
            : base(accountNumber, customerName, balance) { }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            if (Balance + OverdraftLimit < amount)
                throw new OverDraftLimitExceededException($"Withdrawal amount exceeds overdraft limit for account {AccountNumber}");

            Balance -= amount;
        }
    }

    public class HMBank
    {
        private readonly Dictionary<string, Account> _accounts = new Dictionary<string, Account>();

        public void AddAccount(Account account)
        {
            if (account == null)
                throw new NullReferenceException("Account cannot be null");

            _accounts[account.AccountNumber] = account;
        }

        public Account GetAccount(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
                throw new ArgumentNullException(nameof(accountNumber));

            if (!_accounts.TryGetValue(accountNumber, out Account account))
                throw new InvalidAccountException($"Account {accountNumber} not found");

            return account;
        }

        public void Transfer(string fromAccount, string toAccount, decimal amount)
        {
            Account source = GetAccount(fromAccount);
            Account destination = GetAccount(toAccount);
            source.Transfer(destination, amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HMBank bank = new HMBank();

            bank.AddAccount(new SavingsAccount("SAV001", "Ramya S P", 5000));
            bank.AddAccount(new CurrentAccount("CUR001", "Lakshaya M", 2000));
            bank.AddAccount(new SavingsAccount("SAV002", "Madhumitha M", 3000));

            TestScenario(bank, "Valid Transfer", () => bank.Transfer("SAV001", "SAV002", 1000));
            TestScenario(bank, "Invalid Account", () => bank.Transfer("SAV001", "INVALID", 500));
            TestScenario(bank, "Insufficient Funds", () => bank.Transfer("SAV001", "SAV002", 10000));
            TestScenario(bank, "Overdraft Limit", () => bank.Transfer("CUR001", "SAV001", 15000));

            TestScenario(bank, "Null Reference", () =>
            {
                Account nullAccount = null;
               // nullAccount.Deposit(100);
            });
        }

        static void TestScenario(HMBank bank, string description, Action action)
        {
            Console.WriteLine($"\nScenario: {description}");
            try
            {
                action();
                Console.WriteLine("Success!");
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Invalid Account Error: {ex.Message}");
            }
            catch (InsufficientFundException ex)
            {
                Console.WriteLine($"Insufficient Funds Error: {ex.Message}");
            }
            catch (OverDraftLimitExceededException ex)
            {
                Console.WriteLine($"Overdraft Limit Error: {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Null Reference Error: {ex.Message}");
                Console.WriteLine("Please ensure all objects are properly initialized");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }
}