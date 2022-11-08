namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class Vector
    {
        public static Vector operator *(Vector vector, Matrix matrix)
        {
            var result = new double[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            for (int j = 0; j < vector.Length; j++)
                result[i] += matrix[i, j] * vector[j];

            return new Vector(result);
        }

        public virtual double this[int x]
        {
            get => _values[x];
            set => _values[x] = value;
        }

        public int Length => _values.Length;

        private readonly double[] _values;

        public Vector(double[] values)
        {
            _values = values;
        }
    }
}
