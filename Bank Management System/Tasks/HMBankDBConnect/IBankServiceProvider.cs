namespace HMBankDBConnect
{
    public interface IBankServiceProvider : ICustomerServiceProvider
    {
        Account CreateAccount(Customer customer, long accNo, string accType, decimal balance);
        List<Account> ListAccounts();
        void CalculateInterest();
    }
}




