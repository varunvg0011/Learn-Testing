namespace Sparky
{
    public class Calculator
    {


        public List<int> NumberRange { get; set; } = new List<int>();
        public int AddTwoNo(int a, int b)
        {
            return a + b;
        }


        public bool IfSameNo(int a, int b)
        {
            return a == b;
        }

        public bool IsOdd(int a)
        {
            
            return a % 2 != 0;
            
        }


        public double AddTwoDoubleNos(double a, double b)
        {
            return a + b;
        }


        public List<int> GetOddNumbersFromRange(int a, int b)
        {
            NumberRange.Clear();
            for (int i = a; i < b; i++)
            {
                if (i % 2 != 0)
                {
                    NumberRange.Add(i);
                }
            }
            return NumberRange;
        }

    }
}