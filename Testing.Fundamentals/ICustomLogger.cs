namespace Testing.Services
{
    public interface ICustomLogger
    {
        bool LogInformation(string message);

        bool LogInformationToDatabase(double amount, double currentBalance);

        void LogWithOuput(string message, out string output);
    }
}
