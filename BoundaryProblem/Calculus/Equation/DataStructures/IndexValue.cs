namespace BoundaryProblem.Calculus.Equation.DataStructures
{
    public readonly record struct IndexValue(int ColumnIndex, double Value, int ValueIndex);

    public readonly ref struct RefIndexValue
    {
        public int ColumnIndex { get; }
        public double Value => _value[_valueIndex];

        public void SetValue(double value)
        {
            _value[_valueIndex] = value;
        }

        private readonly Span<double> _value;
        private readonly int _valueIndex;

        public RefIndexValue(int columnIndex, Span<double> value, int valueIndex)
        {
            _value = value;
            _valueIndex = valueIndex;
            ColumnIndex = columnIndex;
        }
    }

    public readonly ref struct SparseMatrixRow
    {
        public int Index { get; }
        private readonly ReadOnlySpan<int> _columnIndexes;
        private readonly Span<double> _values;

        public SparseMatrixRow(ReadOnlySpan<int> columnIndexes, Span<double> values, int index)
        {
            Index = index;
            _columnIndexes = columnIndexes;
            _values = values;
        }

        public Enumerator GetEnumerator() => new (_columnIndexes, _values);

        public ref struct Enumerator
        {
            private readonly ReadOnlySpan<int> _columnIndexes;
            private readonly Span<double> _values;
            
            private int _index;

            internal Enumerator(ReadOnlySpan<int> columnIndexes, Span<double> values)
            {
                _columnIndexes = columnIndexes;
                _values = values;
                _index = -1;
            }

            public bool MoveNext()
            {
                int index = _index + 1;
                if (index < _values.Length)
                {
                    _index = index;
                    return true;
                }

                return false;
            }

            public RefIndexValue Current => new (_columnIndexes[_index], _values, _index);
        }

    }
}
