namespace HMBankDBConnect
{
    public class CurrentAccount : Account
    {
        public decimal OverdraftLimit { get; set; }

        public CurrentAccount(Customer customer, decimal balance, decimal overdraftLimit)
            : base(customer, "Current", balance)
        {
            OverdraftLimit = overdraftLimit;
        }

        public override void PrintAccountInfo()
        {
            base.PrintAccountInfo();
            Console.WriteLine($"Overdraft Limit: {OverdraftLimit:C}");
        }
    }

}

