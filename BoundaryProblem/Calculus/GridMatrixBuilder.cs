using BoundaryProblem.Geometry;
using BoundaryProblem.Extensions;

namespace BoundaryProblem.Calculus
{
    public class MatrixBuilder
    {
        private Grid _grid;

        public SparseMatrix FromGrid(Grid grid)
        {
            _grid = grid;

            var indexesLists = GetLowerNodeIndexesLists();

            var rowIndexes = GetRowIndexes(indexesLists);
            var columnIndexes = GetColumnIndexes(indexesLists);

            return new SparseMatrix(rowIndexes, columnIndexes);
            
        }

        private SortedSet<int>[] GetLowerNodeIndexesLists()
        {
            var indexesLists = GetDefaultIndexesLists();

            foreach (var relatedIndexes in _grid.ElementNodeIndexes)
            {
                foreach (var index in relatedIndexes)
                {
                    var lowerIndexes = GetLowerIndexesThan(index, relatedIndexes);
                    indexesLists[index].AddRange(lowerIndexes);
                }
            }

            return indexesLists;
        }

        private SortedSet<int>[] GetDefaultIndexesLists()
        {
            var indexesLists = new SortedSet<int>[_grid.Nodes.Length];
            for (var i = 0; i < indexesLists.Length; i++)
            {
                indexesLists[i] = new SortedSet<int>();
            }

            return indexesLists;
        }

        private static IEnumerable<int> GetLowerIndexesThan(int targetIndex, IEnumerable<int> indexes) => 
            indexes.Where(index => index < targetIndex);

        private static IEnumerable<int> GetRowIndexes(IEnumerable<SortedSet<int>> indexesLists)
        {
            var capacity = 0;
            foreach (var list in indexesLists)
            {
                capacity += list.Count;
                yield return capacity;
            }
        }

        private static IEnumerable<int> GetColumnIndexes(IEnumerable<SortedSet<int>> indexesLists)
        {
            return indexesLists.SelectMany(list => list);
        }
    }
}
