namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class Vector
    {
        public static Vector Create(int length, double defaultValue)
        {
            return Create(length, _ => defaultValue);
        }

        public static Vector Create(int length, Func<int, double> filling)
        {
            var values = new double[length];
            for (int i = 0; i < length; i++)
                values[i] = filling(i);

            return new Vector(values);
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            var result = new double[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            for (int j = 0; j < vector.Length; j++)
                result[i] += matrix[i, j] * vector[j];

            return new Vector(result);
        }

        public static Vector operator *(Vector vector, Matrix matrix) =>
            matrix * vector;

        public virtual double this[int x]
        {
            get => _values[x];
            set => _values[x] = value;
        }

        public Vector Copy()
        {
            var values = new double[Length];
            for (int i = 0; i < Length; i++)
                values[i] = this[i];

            return new Vector(values);
        }

        public int Length => _values.Length;

        private readonly double[] _values;

        public Vector(params double[] values)
        {
            _values = values;
        }
    }
}
