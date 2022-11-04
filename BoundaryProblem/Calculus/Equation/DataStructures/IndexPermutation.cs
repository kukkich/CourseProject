namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public readonly struct IndexPermutation
    {
        public int ApplyRowPermutation(int index) => _rowPermutation[index];
        public int ApplyColumnPermutation(int index) => _columnPermutation[index];
        public int Length => _rowPermutation.Length;

        private readonly int[] _rowPermutation;
        private readonly int[] _columnPermutation;

        public IndexPermutation(int[] rowPermutation, int[] columnPermutation)
        {
            _rowPermutation = rowPermutation;
            _columnPermutation = columnPermutation;
        }

        public IndexPermutation(int[] permutation)
            :this(permutation, permutation) 
            { }
    }
}
