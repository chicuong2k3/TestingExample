namespace Testing.Services
{
    public class BankAccount
    {
        private double balance;
        private readonly ICustomLogger logger;

        public double Balance { get => balance; }

        public BankAccount(ICustomLogger logger)
        {
            balance = 0;
            this.logger = logger;
        }

        public void Deposit(double amount)
        {
            logger.LogInformation($"Depositing {amount}...");
            if (amount <= 0)
            {
                throw new System.ArgumentOutOfRangeException("Deposit amount must be greater than 0.");
            }

            balance += amount;
            logger.LogInformation($"Deposited successfully. Current balance: {Balance}.");
        }

        public bool Withdraw(double amount)
        {
            logger.LogInformation($"Withdrawing {amount}...");
            
            var success = logger.LogInformationToDatabase(amount, balance);

            if (!success)
            {
                return false;
            }

            balance -= amount;
            return true;
        }
    }
}

