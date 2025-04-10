using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    class Bank
    {
        static void Main(string[] args)
        {
            BankServiceProviderImpl bank = new BankServiceProviderImpl("Main Branch", "123 Main St");

            Console.WriteLine("=== Welcome to HMBank System ===");

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Get Balance");
                Console.WriteLine("5. Transfer");
                Console.WriteLine("6. Get Account Details");
                Console.WriteLine("7. List All Accounts");
                Console.WriteLine("8. Get Transactions");
                Console.WriteLine("9. Calculate Interest");
                Console.WriteLine("10. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": CreateAccount(bank); break;
                        case "2": Deposit(bank); break;
                        case "3": Withdraw(bank); break;
                        case "4": GetBalance(bank); break;
                        case "5": Transfer(bank); break;
                        case "6": GetAccountDetails(bank); break;
                        case "7": ListAccounts(bank); break;
                        case "8": GetTransactions(bank); break;
                        case "9": CalculateInterest(bank); break;
                        case "10": return;
                        default: Console.WriteLine("Invalid option."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void CreateAccount(BankServiceProviderImpl bank)
        {
            Console.WriteLine("Select Account Type:");
            Console.WriteLine("1. Savings Account");
            Console.WriteLine("2. Current Account");
            Console.WriteLine("3. Zero Balance Account");
            Console.Write("Choice: ");
            string accTypeChoice = Console.ReadLine();

            Console.Write("Customer Name: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();

            Console.Write("Initial Balance: ");
            decimal balance = decimal.Parse(Console.ReadLine());

            string accType = accTypeChoice switch
            {
                "1" => "Savings",
                "2" => "Current",
                "3" => "ZeroBalance",
                _ => throw new ArgumentException("Invalid account type")
            };

            Customer customer = new Customer(name, email, phone, address);
            bank.CreateAccount(customer, accType, balance);
            Console.WriteLine("Account created successfully!");
        }

        static void Deposit(BankServiceProviderImpl bank)
        {
            Console.Write("Enter Account Number: ");
            long accNo = long.Parse(Console.ReadLine());
            Console.Write("Enter Deposit Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            bank.Deposit(accNo, amount);
            Console.WriteLine($"Deposit successful. New Balance: {bank.GetAccountBalance(accNo):C}");
        }

        static void Withdraw(BankServiceProviderImpl bank)
        {
            Console.Write("Enter Account Number: ");
            long accNo = long.Parse(Console.ReadLine());
            Console.Write("Enter Withdraw Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            bank.Withdraw(accNo, amount);
            Console.WriteLine($"Withdrawal successful. New Balance: {bank.GetAccountBalance(accNo):C}");
        }

        static void GetBalance(BankServiceProviderImpl bank)
        {
            Console.Write("Enter Account Number: ");
            long accNo = long.Parse(Console.ReadLine());

            decimal balance = bank.GetAccountBalance(accNo);
            Console.WriteLine($"Current Balance: {balance:C}");
        }

        static void Transfer(BankServiceProviderImpl bank)
        {
            Console.Write("Enter From Account Number: ");
            long fromAcc = long.Parse(Console.ReadLine());
            Console.Write("Enter To Account Number: ");
            long toAcc = long.Parse(Console.ReadLine());
            Console.Write("Enter Transfer Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            bank.Transfer(fromAcc, toAcc, amount);
            Console.WriteLine("Transfer completed successfully.");
        }

        static void GetAccountDetails(BankServiceProviderImpl bank)
        {
            Console.Write("Enter Account Number: ");
            long accNo = long.Parse(Console.ReadLine());

            Account acc = bank.GetAccountDetails(accNo);
            Console.WriteLine(acc.ToString());
        }

        static void ListAccounts(BankServiceProviderImpl bank)
        {
            List<Account> accounts = bank.ListAccounts();
            Console.WriteLine("\nAll Accounts:");
            foreach (var acc in accounts)
            {
                Console.WriteLine(acc.ToString());
            }
        }

        static void GetTransactions(BankServiceProviderImpl bank)
        {
            Console.Write("Enter Account Number: ");
            long accNo = long.Parse(Console.ReadLine());

            Console.Write("From Date (yyyy-MM-dd): ");
            DateTime fromDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.Write("To Date (yyyy-MM-dd): ");
            DateTime toDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            List<Transaction> transactions = bank.GetTransactions(accNo, fromDate, toDate);

            Console.WriteLine($"\nTransactions for Account {accNo}:");
            foreach (var txn in transactions)
            {
                Console.WriteLine($"{txn.TransactionDate} | {txn.TransactionType} | {txn.TransactionAmount:C} | {txn.Description}");
            }
        }

        static void CalculateInterest(BankServiceProviderImpl bank)
        {
            bank.CalculateInterest();
            Console.WriteLine("Interest calculated for all savings accounts.");
        }
    }
}


