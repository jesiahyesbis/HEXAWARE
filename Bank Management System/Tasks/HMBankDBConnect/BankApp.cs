using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HMBankDBConnect
{
    public class BankApp
    {
        private static IBankRepository bankRepository = new BankRepositoryImpl();

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Banking System!");

            while (true)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateAccountMenu();
                        break;
                    case "2":
                        DepositMenu();
                        break;
                    case "3":
                        WithdrawMenu();
                        break;
                    case "4":
                        GetBalanceMenu();
                        break;
                    case "5":
                        TransferMenu();
                        break;
                    case "6":
                        GetAccountDetailsMenu();
                        break;
                    case "7":
                        ListAccountsMenu();
                        break;
                    case "8":
                        GetTransactionsMenu();
                        break;
                    case "9":
                        CalculateInterestMenu();
                        break;
                    case "10":
                        Console.WriteLine("Thank you for using the Banking System. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }


                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("MAIN MENU");
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
            Console.Write("Enter your choice: ");
        }

        private static void CreateAccountMenu()
        {
            Console.Clear();
            Console.WriteLine("CREATE ACCOUNT");

            try
            {
                // Get customer details
                Console.Write("First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Email Address: ");
                string email = Console.ReadLine();

                Console.Write("Phone Number (10 digits): ");
                string phone = Console.ReadLine();

                Console.Write("Address: ");
                string address = Console.ReadLine();

                // Create customer object
                Customer customer = new Customer(0, firstName, lastName, email, phone, address);

                // Get account type
                Console.WriteLine("\nSelect Account Type:");
                Console.WriteLine("1. Savings Account (Min balance 500)");
                Console.WriteLine("2. Current Account (With overdraft)");
                Console.WriteLine("3. Zero Balance Account");
                Console.Write("Enter choice: ");
                string accTypeChoice = Console.ReadLine();

                string accountType = "";
                decimal balance = 0;

                switch (accTypeChoice)
                {
                    case "1":
                        accountType = "Savings";
                        Console.Write("Enter initial deposit (minimum 500): ");
                        balance = decimal.Parse(Console.ReadLine());
                        break;
                    case "2":
                        accountType = "Current";
                        Console.Write("Enter initial balance: ");
                        balance = decimal.Parse(Console.ReadLine());
                        break;
                    case "3":
                        accountType = "ZeroBalance";
                        balance = 0;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Account creation cancelled.");
                        return;
                }

                // Create account
                Account account = bankRepository.CreateAccount(customer, DBUtil.GetLastInsertedAccountNumber() + 1, accountType, balance);

                Console.WriteLine("\nAccount created successfully!");
                Console.WriteLine($"Account Number: {account.AccountNumber-1}");
                Console.WriteLine($"Account Type: {account.AccountType}");
                Console.WriteLine($"Balance: {account.Balance:C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
            }
        }

        private static void DepositMenu()
        {
            Console.Clear();
            Console.WriteLine("DEPOSIT");

            try
            {
                Console.Write("Enter Account Number: ");
                long accountNumber = long.Parse(Console.ReadLine());

                Console.Write("Enter Amount to Deposit: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                decimal newBalance = bankRepository.Deposit(accountNumber, amount);

                Console.WriteLine($"Deposit successful. New balance: {newBalance:C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deposit: {ex.Message}");
            }
        }

        private static void WithdrawMenu()
        {
            Console.Clear();
            Console.WriteLine("WITHDRAW");

            try
            {
                Console.Write("Enter Account Number: ");
                long accountNumber = long.Parse(Console.ReadLine());

                Console.Write("Enter Amount to Withdraw: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                decimal newBalance = bankRepository.Withdraw(accountNumber, amount);

                Console.WriteLine($"Withdrawal successful. New balance: {newBalance:C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during withdrawal: {ex.Message}");
            }
        }

        private static void GetBalanceMenu()
        {
            Console.Clear();
            Console.WriteLine("GET BALANCE");

            try
            {
                Console.Write("Enter Account Number: ");
                long accountNumber = long.Parse(Console.ReadLine());

                decimal balance = bankRepository.GetAccountBalance(accountNumber);

                Console.WriteLine($"Current balance: {balance:C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting balance: {ex.Message}");
            }
        }

        private static void TransferMenu()
        {
            Console.Clear();
            Console.WriteLine("TRANSFER");

            try
            {
                Console.Write("Enter From Account Number: ");
                long fromAccount = long.Parse(Console.ReadLine());

                Console.Write("Enter To Account Number: ");
                long toAccount = long.Parse(Console.ReadLine());

                Console.Write("Enter Amount to Transfer: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                bool success = bankRepository.Transfer(fromAccount, toAccount, amount);

                if (success)
                {
                    Console.WriteLine("Transfer completed successfully.");
                    Console.WriteLine($"From Account New Balance: {bankRepository.GetAccountBalance(fromAccount):C}");
                    Console.WriteLine($"To Account New Balance: {bankRepository.GetAccountBalance(toAccount):C}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during transfer: {ex.Message}");
            }
        }

        private static void GetAccountDetailsMenu()
        {
            Console.Clear();
            Console.WriteLine("ACCOUNT DETAILS");

            try
            {
                Console.Write("Enter Account Number: ");
                long accountNumber = long.Parse(Console.ReadLine());

                Account account = bankRepository.GetAccountDetails(accountNumber);

                Console.WriteLine("\nAccount Details:");
                Console.WriteLine($"Account Number: {account.AccountNumber}");
                Console.WriteLine($"Account Type: {account.AccountType}");
                Console.WriteLine($"Balance: {account.Balance:C}");

                Console.WriteLine("\nCustomer Details:");
                Console.WriteLine($"Customer ID: {account.Customer.CustomerID}");
                Console.WriteLine($"Name: {account.Customer.FirstName} {account.Customer.LastName}");
                Console.WriteLine($"Email: {account.Customer.EmailAddress}");
                Console.WriteLine($"Phone: {account.Customer.PhoneNumber}");
                Console.WriteLine($"Address: {account.Customer.Address}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting account details: {ex.Message}");
            }
        }

        private static void ListAccountsMenu()
        {
            Console.Clear();
            Console.WriteLine("LIST ALL ACCOUNTS");

            try
            {
                List<Account> accounts = bankRepository.ListAccounts();

                if (accounts.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                foreach (var account in accounts)
                {
                    Console.WriteLine("\n----------------------------------------");
                    Console.WriteLine($"Account Number: {account.AccountNumber}");
                    Console.WriteLine($"Account Type: {account.AccountType}");
                    Console.WriteLine($"Balance: {account.Balance:C}");
                    Console.WriteLine($"Customer: {account.Customer.FirstName} {account.Customer.LastName}");

                    if (account is SavingsAccount savings)
                        Console.WriteLine($"Interest Rate: {savings.InterestRate}%");
                    else if (account is CurrentAccount current)
                        Console.WriteLine($"Overdraft Limit: {current.OverdraftLimit:C}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing accounts: {ex.Message}");
            }
        }

        private static void GetTransactionsMenu()
        {
            Console.Clear();
            Console.WriteLine("GET TRANSACTIONS");

            try
            {
                Console.Write("Enter Account Number: ");
                long accountNumber = long.Parse(Console.ReadLine());

                Console.Write("Enter Start Date (yyyy-mm-dd): ");
                DateTime fromDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Enter End Date (yyyy-mm-dd): ");
                DateTime toDate = DateTime.Parse(Console.ReadLine());

                List<Transaction> transactions = bankRepository.GetTransactions(accountNumber, fromDate, toDate);

                if (transactions.Count == 0)
                {
                    Console.WriteLine("No transactions found for the given period.");
                    return;
                }

                Console.WriteLine($"\nTransactions for Account {accountNumber} from {fromDate:d} to {toDate:d}:");
                foreach (var transaction in transactions)
                {
                    Console.WriteLine("\n----------------------------------------");
                    Console.WriteLine($"Date: {transaction.TransactionDate}");
                    Console.WriteLine($"Type: {transaction.TransactionType}");
                    Console.WriteLine($"Amount: {transaction.TransactionAmount:C}");
                    Console.WriteLine($"Description: {transaction.Description}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transactions: {ex.Message}");
            }
        }

        private static void CalculateInterestMenu()
        {
            Console.Clear();
            Console.WriteLine("CALCULATE INTEREST");

            try
            {
                bankRepository.CalculateInterest();
                Console.WriteLine("Interest calculated and applied to all savings accounts.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating interest: {ex.Message}");
            }
        }
    }
}











