using BoundaryProblem.Calculus.Equation.DataStructures;

namespace BoundaryProblem.Calculus.Equation
{
    public class MatrixInserter
    {
        public void Insert(SymmetricSparseMatrix sparseMatrix, LocalMatrix localMatrix)
        {
            var matrixSize = localMatrix.IndexPermutation.Length;
            for (var i = 0; i < matrixSize; i++)
            {
                var row = localMatrix.IndexPermutation
                    .ApplyRowPermutation(i);

                for (var j = 0; j < matrixSize; j++)
                {
                    var column = localMatrix.IndexPermutation
                        .ApplyColumnPermutation(j);
                    if (column > row) continue;

                    sparseMatrix[row, column] = localMatrix[i, j];
                }
            }
        }
    }
}
