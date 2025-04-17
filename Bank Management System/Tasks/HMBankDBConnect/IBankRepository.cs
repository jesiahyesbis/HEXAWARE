namespace HMBankDBConnect
{
    public interface IBankRepository
    {
        Account CreateAccount(Customer customer, long accNo, string accType, decimal balance);
        List<Account> ListAccounts();
        void CalculateInterest();
        decimal GetAccountBalance(long accountNumber);
        decimal Deposit(long accountNumber, decimal amount);
        decimal Withdraw(long accountNumber, decimal amount);
        bool Transfer(long fromAccountNumber, long toAccountNumber, decimal amount);
        Account GetAccountDetails(long accountNumber);
        List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate);
    }


}



