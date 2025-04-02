using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class BankApp
    {
        private static Bank bank = new Bank();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- Bank Menu ---");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Get Account Balance");
                Console.WriteLine("5. Transfer");
                Console.WriteLine("6. Get Account Details");
                Console.WriteLine("7. Exit");
                Console.WriteLine("Enter the number of the operation to perform");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        Deposit();
                        break;
                    case 3:
                        Withdraw();
                        break;
                    case 4:
                        GetAccountBalance();
                        break;
                    case 5:
                        Transfer();
                        break;
                    case 6:
                        GetAccountDetails();
                        break;
                    case 7:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private static void CreateAccount()
        {
            Console.WriteLine("Enter Customer Name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter Account Type (Savings/Current):");
            string accountType = Console.ReadLine();

            Console.WriteLine("Enter Initial Balance:");
            double balance = double.Parse(Console.ReadLine());

            var customer = new Customer(1001, name, "LastName", "email@example.com", "1234567890", "Some Address");

            bank.CreateAccount(customer, accountType, balance);
        }

        private static void Deposit()
        {
            Console.WriteLine("Enter Account Number:");
            long accountNumber = long.Parse(Console.ReadLine());

            Console.WriteLine("Enter Deposit Amount:");
            double amount = double.Parse(Console.ReadLine());

            bank.Deposit(accountNumber, amount);
        }

        private static void Withdraw()
        {
            Console.WriteLine("Enter Account Number:");
            long accountNumber = long.Parse(Console.ReadLine());

            Console.WriteLine("Enter Withdrawal Amount:");
            double amount = double.Parse(Console.ReadLine());

            bank.Withdraw(accountNumber, amount);
        }

        private static void GetAccountBalance()
        {
            Console.WriteLine("Enter Account Number:");
            long accountNumber = long.Parse(Console.ReadLine());

            double balance = bank.GetAccountBalance(accountNumber);
            Console.WriteLine($"Account Balance: {balance}");
        }

        private static void Transfer()
        {
            Console.WriteLine("Enter From Account Number:");
            long fromAccountNumber = long.Parse(Console.ReadLine());

            Console.WriteLine("Enter To Account Number:");
            long toAccountNumber = long.Parse(Console.ReadLine());

            Console.WriteLine("Enter Transfer Amount:");
            double amount = double.Parse(Console.ReadLine());

            bank.Transfer(fromAccountNumber, toAccountNumber, amount);
        }

        private static void GetAccountDetails()
        {
            Console.WriteLine("Enter Account Number:");
            long accountNumber = long.Parse(Console.ReadLine());

            bank.GetAccountDetails(accountNumber);
        }
    }
}
