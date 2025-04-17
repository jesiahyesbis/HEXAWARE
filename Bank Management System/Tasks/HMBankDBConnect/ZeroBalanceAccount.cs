namespace HMBankDBConnect{
    public class ZeroBalanceAccount : Account
{
    public ZeroBalanceAccount(Customer customer)
        : base(customer, "ZeroBalance", 0)
    {
    }
}}









