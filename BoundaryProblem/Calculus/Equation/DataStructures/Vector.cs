namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class Vector
    {
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
