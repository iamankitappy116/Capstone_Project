namespace BankAccountLib
{
    public class BankAccount
    {
        public decimal Balance { get; private set; }
        public BankAccount(decimal initialBalance)
        {
            Balance = initialBalance;
        }
        public void TransferTo(BankAccount target, decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be positive");
            }
            if(amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds");
            }
            Balance -= amount;
            target.Balance += amount;
        }

    }
}
