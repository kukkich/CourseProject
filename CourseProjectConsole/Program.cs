using BoundaryProblem.Calculus.Equation.DataStructures;

namespace CourseProjectConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] indexes = {0, 1, 0, 1, 3, 2};
            double[] values = {0, 1, 2, 3, 4, 5,};

            var x = new SparseMatrixRow(
                new ReadOnlySpan<int>(indexes, 2, 3),
                new Span<double>(values, 2, 3),
                2
            );

            foreach (RefIndexValue indexValue in x)
            {
                indexValue.SetValue(0);
            }

            Console.ReadLine();
        }

        private static int Mu(int i) => i % 4;

        private static int Nu(int i) => i / 4;

    }
}