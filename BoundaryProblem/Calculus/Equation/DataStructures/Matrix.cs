namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class Matrix
    {
        public double this[int x, int y] => _values[x, y] * _coefficient;
        public int RowLength => _values.GetLength(0);

        private readonly double[,] _values;
        private double _coefficient;

        public Matrix(double[,] values)
        {
            _values = values;
            _coefficient = 1;
        }

        public void Multiply(double coefficient) => _coefficient *= coefficient;

    }
}
