namespace WebApplication
{
    public class CalculateService : ICalculateService
    {
        public int GetSum(int a, int b)
        {
            return a + b;   
        }
    }
}
