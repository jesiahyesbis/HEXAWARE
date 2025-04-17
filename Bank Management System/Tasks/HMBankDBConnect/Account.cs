
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public abstract class Account
    {
        private static long lastAccNo = DBUtil.GetLastInsertedAccountNumber();

        public long AccountNumber { get;  set; }
        public string AccountType { get;  set; }
        public decimal Balance { get;  set; }
        public Customer Customer { get; set; }

        protected Account(Customer customer, string accountType, decimal balance)
        {
            AccountNumber = ++lastAccNo;
            AccountType = accountType;
            Balance = balance;
            Customer = customer;
        }

        public virtual void PrintAccountInfo()
        {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Type: {AccountType}");
            Console.WriteLine($"Balance: {Balance:C}");
            Customer.PrintCustomerInfo();
        }
    }
}



