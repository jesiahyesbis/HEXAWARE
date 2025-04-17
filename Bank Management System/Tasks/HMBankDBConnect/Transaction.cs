using System;
namespace HMBankDBConnect
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }

        public Transaction(Account account, string description, string transactionType, decimal amount)
        {
            Account = account;
            Description = description;
            TransactionType = transactionType;
            TransactionAmount = amount;
            TransactionDate = DateTime.Now;
        }

        public void PrintTransactionInfo()
        {
            Console.WriteLine($"Transaction ID: {TransactionID}");
            Console.WriteLine($"Account Number: {Account.AccountNumber}");
            Console.WriteLine($"Type: {TransactionType}");
            Console.WriteLine($"Amount: {TransactionAmount:C}");
            Console.WriteLine($"Date: {TransactionDate}");
            Console.WriteLine($"Description: {Description}");
        }
    }
}





