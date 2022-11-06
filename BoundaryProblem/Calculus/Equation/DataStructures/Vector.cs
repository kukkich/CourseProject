namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class Vector
    {
        public virtual ref double this[int x] => ref _values[x];

        public int Length => _values.Length;

        private readonly double[] _values;

        public Vector(double[] values)
        {
            _values = values;
        }
    }
}
