using BoundaryProblem.Calculus.Equation.Assembling;
using BoundaryProblem.Calculus.Equation.DataStructures;

namespace CourseProjectConsole
{
    internal class IdentityMatrix : Matrix
    {
        private const int Size = 16;
        private static readonly double[,] IdentityValues;

        static IdentityMatrix()
        {
            IdentityValues = new double[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                IdentityValues[i, i] = 1;
            }
        }

        public IdentityMatrix()
            : base(IdentityValues)
        { }
    }

    internal class Program
    {
        private static int[] _nodeIndexes;
        private static LocalRightSideAssembler _assembler;

        static void Main(string[] args)
        {
            var x = new Span<int>(new int[] { 1, 2, 5, 5, 5, 6, 7, 9, 9, 13, 23});
            var z = x.BinarySearch(8);

            Console.ReadLine();
        }

        private static int Mu(int i) => i % 4;

        private static int Nu(int i) => i / 4;

    }
}