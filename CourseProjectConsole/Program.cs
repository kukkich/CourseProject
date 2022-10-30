using BoundaryProblem.Calculus.Equation.DataStructures;

namespace CourseProjectConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new (new double[,] {{1, 2, 3, 322}, {4, 5, 6, 1488}, {900, 10, -42, 12}});

            Console.WriteLine(matrix.RowLength);

            Console.WriteLine("Hello, World!");
            for (int i = 0; i < 16; i++)
            {
                Console.Write(i);
                Console.Write(": [");
                Console.Write(XIndex(i));
                Console.Write(", ");
                Console.Write(YIndex(i));
                Console.WriteLine("]");
            }
        }

        private static int XIndex(int i)
        {
            return i % 4;
        }

        private static int YIndex(int i)
        {
            return i / 4;
        }

    }
}