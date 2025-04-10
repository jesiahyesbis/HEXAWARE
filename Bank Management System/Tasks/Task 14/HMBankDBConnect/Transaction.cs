using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class Transaction
    {
        public long AccountNumber { get; }
        public string Description { get; }
        public DateTime TransactionDate { get; }
        public string TransactionType { get; }
        public decimal TransactionAmount { get; }

        public Transaction(long accountNumber, string description,
                         DateTime transactionDate, string transactionType,
                         decimal transactionAmount)
        {
            AccountNumber = accountNumber;
            Description = description;
            TransactionDate = transactionDate;
            TransactionType = transactionType;
            TransactionAmount = transactionAmount;
        }

        public override string ToString()
        {
            return $"{TransactionDate} | {TransactionType} | {TransactionAmount:C} | {Description}";
        }
    }

}
