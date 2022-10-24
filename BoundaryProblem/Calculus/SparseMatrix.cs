namespace BoundaryProblem.Calculus
{
    public class SparseMatrix
    {
        public ReadOnlySpan<int> RowIndexes => new (_rowIndexes);
        public ReadOnlySpan<int> ColumnIndexes => new (_columnIndexes);

        private readonly int[] _rowIndexes;
        private readonly int[] _columnIndexes;

        public SparseMatrix(IEnumerable<int> rowIndexes, IEnumerable<int> columnIndexes)
        {
            _rowIndexes = rowIndexes.ToArray();
            _columnIndexes = columnIndexes.ToArray();
        }
    }
}
