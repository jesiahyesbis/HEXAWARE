using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class ZeroBalanceAccount : Account
    {
        public ZeroBalanceAccount(Customer customer)
            : base("ZeroBalance", 0, customer)
        {
        }

        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient balance");

            Balance -= amount;
        }

        public override string ToString()
        {
            return base.ToString() + " (Zero Balance Account)";
        }
    }

}
