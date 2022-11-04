namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public readonly struct Matrix
    {
        public double this[int x, int y] => _values[x, y] * _coefficient;
        public int RowLength => _values.GetLength(0);

        private readonly double[,] _values;
        private readonly double _coefficient;

        public Matrix(double[,] values)
        {
            _values = values;
            _coefficient = 1;
        }

        private Matrix(double[,] values, double coefficient)
        {
            _values = values;
            _coefficient = coefficient;
        }

        public static Matrix operator *(Matrix matrix, double coefficient)
        {
            return new Matrix(matrix._values, matrix._coefficient * coefficient);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.RowLength != b.RowLength) throw new ArgumentException();

            var values = new double[a.RowLength, a.RowLength];
            
            for (int i = 0; i < a.RowLength; i++)
                for (int j = 0; j < a.RowLength; j++)
                    values[i, j] = a[i, j] + b[i, j];

            return new Matrix(values);
        }
    }
}
