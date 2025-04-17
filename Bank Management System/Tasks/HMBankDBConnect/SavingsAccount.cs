namespace HMBankDBConnect
{
    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; set; }

        public SavingsAccount(Customer customer, decimal balance, decimal interestRate)
            : base(customer, "Savings", balance)
        {
            if (balance < 500)
                throw new ArgumentException("Savings account must be created with minimum balance of 500.");

            InterestRate = interestRate;
        }

        public override void PrintAccountInfo()
        {
            base.PrintAccountInfo();
            Console.WriteLine($"Interest Rate: {InterestRate}%");
        }
    }

}


