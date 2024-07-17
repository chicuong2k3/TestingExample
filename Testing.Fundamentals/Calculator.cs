namespace Testing.Fundamentals
{
    public class Calculator
    {
        public event EventHandler<int>? SpeedSet;
        public int Speed { get; set; }
        public void SetSpeed(int speed)
        {
            if (speed < 0)
            {
                throw new ArgumentException("The speed should be greater than 0.");
            }

            Speed = speed;

            SpeedSet?.Invoke(this, speed);
        }
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiple(int a, int b) => a * b;
        public int Divide(int a, int b) => a / b;
        public int Max(int a, int b) => a >= b ? a : b;

        public IEnumerable<int> GetEvenNumbers(int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                if (i % 2 == 0)
                {
                    yield return i;
                }
            }
        }
    }
}
