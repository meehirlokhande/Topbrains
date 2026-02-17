namespace Domain
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string HolderName { get; set; }
        public decimal Balance { get; set; }

        public Account(string accountNumber, string holderName, decimal balance)
        {
            AccountNumber = accountNumber;
            HolderName = holderName;
            Balance = balance;
        }
    }
}
