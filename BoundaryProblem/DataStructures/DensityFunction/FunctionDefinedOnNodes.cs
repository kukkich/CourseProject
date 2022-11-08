namespace BoundaryProblem.DataStructures.DensityFunction
{
    public class FunctionDefinedOnNodes : IDensityFunctionProvider
    {
        private readonly Dictionary<int, double> _values;

        public FunctionDefinedOnNodes(Dictionary<int, double> values)
        {
            _values = values;
        }

        public double Calc(int globalNodeIndex)
        {
            if (_values.TryGetValue(globalNodeIndex, out var value))
                return value;

            throw new ArgumentOutOfRangeException($"There no node with index {globalNodeIndex}");
        }
    }
}
