namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class LocalMatrix
    {
        public double this[int x, int y] => _matrix[x, y];
        public readonly IndexPermutation IndexPermutation;

        private readonly Matrix _matrix;

        public LocalMatrix(Matrix matrix, IndexPermutation permutation)
        {
            _matrix = matrix;
            IndexPermutation = permutation;
        }

    }
}
