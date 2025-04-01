//Task 1: Conditional Statements
//Create a program that checks if a customer is eligible for a
// loan based on their credit score and annual income.

using System;

internal class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter your credit score: ");
        int creditScore = int.Parse(Console.ReadLine());

        Console.Write("Enter your annual income: ");
        double annualIncome = double.Parse(Console.ReadLine());

        if (creditScore > 700 && annualIncome >= 50000)
        {
            Console.WriteLine("You are eligible for a loan.");
        }
        else
        {
            Console.WriteLine("You are not eligible for a loan.");
        }
    }
}




//Task 2: Nested Conditional Statements
//Create a program that simulates an ATM transaction with options like "Check Balance", "Withdraw", "Deposit". 
//It checks if the withdrawal is in multiples of 100 or 500 and ensures sufficient balance.


using System;

class Program
{
    static void Main()
    {
        double balance = 1000;
        Console.WriteLine("ATM Options:");
        Console.WriteLine("1. Check Balance");
        Console.WriteLine("2. Withdraw");
        Console.WriteLine("3. Deposit");
        Console.Write("Enter your choice (1/2/3): ");
        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
        {
            Console.WriteLine($"Your balance is: ${balance}");
        }
        else if (choice == 2)
        {
            Console.Write("Enter amount to withdraw: ");
            double withdrawAmount = double.Parse(Console.ReadLine());

            if (withdrawAmount > balance)
            {
                Console.WriteLine("Insufficient balance.");
            }
            else if (withdrawAmount % 100 == 0 || withdrawAmount % 500 == 0)
            {
                balance -= withdrawAmount;
                Console.WriteLine($"Withdrawal successful. New balance: ${balance}");
            }
            else
            {
                Console.WriteLine("Withdrawal amount must be in multiples of 100 or 500.");
            }
        }
        else if (choice == 3)
        {
            Console.Write("Enter amount to deposit: ");
            double depositAmount = double.Parse(Console.ReadLine());
            balance += depositAmount;
            Console.WriteLine($"Deposit successful. New balance: ${balance}");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}


//Task 3: Loop Structures
//Create a program that calculates the future balance for each customer's savings 
//account after a certain number of years using compound interest.

using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter the number of customers: ");
        int numberOfCustomers = int.Parse(Console.ReadLine());

        for (int i = 1; i <= numberOfCustomers; i++)
        {
            Console.WriteLine($"\nCustomer {i}:");
            Console.Write("Enter the initial balance: ");
            double initialBalance = double.Parse(Console.ReadLine());

            Console.Write("Enter the annual interest rate (in percentage): ");
            double annualInterestRate = double.Parse(Console.ReadLine());

            Console.Write("Enter the number of years: ");
            int years = int.Parse(Console.ReadLine());

            double futureBalance = initialBalance * Math.Pow((1 + annualInterestRate / 100), years);
            Console.WriteLine($"Future balance after {years} years: ${futureBalance:F2}");
        }
    }
}



//Task 4: Looping, Array and Data Validation
//Create a program to validate customer account numbers and display balances. 
//The account number format should be `INDB` followed by 4 numbers (e.g., `INDB2345`).

using System;

class Program
{
    static void Main()
    {
        string validAccountNumber = "INDB2345";
        double accountBalance = 1000.00;
        string enteredAccountNumber;

        do
        {
            Console.Write("Enter your account number (INDB followed by 4 digits): ");
            enteredAccountNumber = Console.ReadLine();

            if (enteredAccountNumber == validAccountNumber)
            {
                Console.WriteLine($"Account balance: ${accountBalance}");
                break;
            }
            else
            {
                Console.WriteLine("Invalid account number. Please try again.");
            }
        }
        while (true);
    }
}



//Task 5: Password Validation (First Version)
//Create a program to validate a password based on specific rules.

using System;

class Program
{
    static void Main()
    {
        Console.Write("Create a password: ");
        string password = Console.ReadLine();

        if (password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsDigit))
        {
            Console.WriteLine("Password is valid.");
        }
        else
        {
            Console.WriteLine("Password is invalid.");
        }
    }
}


//Task 6: Password Validation 
//Create a program to maintain a list of transactions (deposits and withdrawals) and display the transaction history.


using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<string> transactions = new List<string>();
        string userChoice;

        do
        {
            Console.WriteLine("Enter a transaction (Deposit or Withdrawal): ");
            string transaction = Console.ReadLine();

            Console.WriteLine("Enter the amount: ");
            double amount = double.Parse(Console.ReadLine());

            if (transaction.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
            {
                transactions.Add($"Deposited: ${amount}");
            }
            else if (transaction.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))
            {
                transactions.Add($"Withdrew: ${amount}");
            }
            else
            {
                Console.WriteLine("Invalid transaction type.");
            }

            Console.WriteLine("Would you like to add another transaction? (yes/no): ");
            userChoice = Console.ReadLine();

        } while (userChoice.Equals("yes", StringComparison.OrdinalIgnoreCase));

        Console.WriteLine("\nTransaction History:");
        foreach (string transaction in transactions)
        {
            Console.WriteLine(transaction);
        }
    }
}
