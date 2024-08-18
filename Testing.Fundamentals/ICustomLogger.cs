namespace Testing.Services
{
    public interface ICustomLogger
    {
        public string LogType { get; set; }
        bool LogInformation(string message);

        bool LogInformationToDatabase(double amount, double currentBalance);

        void LogWithOuput(string message, out string output);
    }
}
