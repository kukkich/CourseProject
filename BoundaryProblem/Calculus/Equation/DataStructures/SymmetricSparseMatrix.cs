namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public class SymmetricSparseMatrix
    {
        public IEnumerable<IndexValue> this[int rowIndex] 
        {
            get
            {
                if (rowIndex < 0) throw new ArgumentOutOfRangeException(nameof(rowIndex));

                var end = _rowIndexes[rowIndex];

                var begin = rowIndex == 0
                    ? 0
                    : _columnIndexes[rowIndex - 1];

                for (var i = begin; i < end; i++)
                    yield return new IndexValue(_columnIndexes[i], Values[i]);
            }
        }

        public ReadOnlySpan<int> RowIndexes => new(_rowIndexes);
        public ReadOnlySpan<int> ColumnIndexes => new(_columnIndexes);
        public double[] Diagonal { get; }
        public double[] Values { get; }

        private readonly int[] _rowIndexes;
        private readonly int[] _columnIndexes;

        public SymmetricSparseMatrix(
            IEnumerable<int> rowIndexes, IEnumerable<int> columnIndexes,
            IEnumerable<double> values,
            IEnumerable<double> diagonal
            )
        {
            _rowIndexes = rowIndexes.ToArray();
            _columnIndexes = columnIndexes.ToArray();
            Values = values.ToArray();
            Diagonal = diagonal.ToArray();

            if (Values.Length != _columnIndexes.Length) throw new ArgumentException(
                nameof(columnIndexes) + " and " + nameof(values) + "must have the same length"
                );
            if (Diagonal.Length != _rowIndexes.Length) throw new ArgumentException(
                nameof(rowIndexes) + " and " + nameof(diagonal) + "must have the same length"
            );
        }

        public SymmetricSparseMatrix(
            IEnumerable<int> rowIndexes, IEnumerable<int> columnIndexes
        )
        {
            _rowIndexes = rowIndexes.ToArray();
            _columnIndexes = columnIndexes.ToArray();
            Diagonal = new double[_columnIndexes.Length];
            Values = new double[_rowIndexes.Length];
        }
    }
}
