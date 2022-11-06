using BoundaryProblem.Calculus.Equation.DataStructures.LocalObjects;
using BoundaryProblem.Calculus.Equation.DataStructures;

namespace BoundaryProblem.Calculus.Equation.Assembling
{
    public class VectorInserter
    {
        public void Insert(Vector vector, LocalVector localVector)
        {
            var vectorLength = localVector.IndexPermutation.Length;
            for (var i = 0; i < vectorLength; i++)
            {
                var row = localVector.IndexPermutation
                    .ApplyPermutation(i);
                vector[row] += localVector[i];
            }
        }
    }
}
