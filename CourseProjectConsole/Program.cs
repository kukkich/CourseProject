using BoundaryProblem.Calculus.Equation.Assembling;
using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.DensityFunction;

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
            Console.ReadLine();
        }

        private static int Mu(int i) => i % 4;

        private static int Nu(int i) => i / 4;

    }
}