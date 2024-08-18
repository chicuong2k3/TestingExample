namespace Testing.Services
{
    public class BankAccountLogger : ICustomLogger
    {
        public string LogType { get; set; }

        public bool LogInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
            return true;
        }

        public bool LogInformationToDatabase(double amount, double currentBalance)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Logging information to database...");

            if (amount <= 0 || amount > currentBalance)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Logged successfully.");
                Console.ResetColor();
                return false;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Logged successfully.");
            Console.ResetColor();

            return true;
        }

        public void LogWithOuput(string message, out string output)
        {
            output = message.ToUpper();
        }
    }
}
