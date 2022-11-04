using BoundaryProblem.Calculus.Equation.DataStructures;

namespace CourseProjectConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new(new double[,] { { 1, 2, 3, 322 }, { 4, 5, 6, 1488 }, { 900, 10, -42, 12 } });

            Console.WriteLine(matrix.RowLength);

            Console.WriteLine("Hello, World!");
            for (int i = 0; i < 16; i++)
            {
                Console.Write(i);
                Console.Write(": [");
                Console.Write(Mu(i));
                Console.Write(", ");
                Console.Write(Nu(i));
                Console.WriteLine("]");
            }
        }

        private static int Mu(int i) => i % 4;

        private static int Nu(int i) => i / 4;

    }
}