using BoundaryProblem.Calculus.Equation.DataStructures;

namespace CourseProject.Tests.MoqDataStructures
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
                for (int j = 0; j < Size; j++)
                {
                    IdentityValues[i, j] = 1;
                }
            }
        }

        public IdentityMatrix()
            : base(IdentityValues)
            { }
    }
}
