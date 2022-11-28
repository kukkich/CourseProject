using BoundaryProblem.Calculus.Equation.DataStructures;

namespace CourseProject.Tests.Asserts;

public static class SparseMatrixAssert
{
    public static void RowEqual(SparseMatrixRow row, double[] expected)
    {
        var rowValues = CloneRowValues(row);

        CollectionAssert.AreEqual(rowValues, expected);
    }

    private static List<double> CloneRowValues(SparseMatrixRow row)
    {
        var result = new List<double>();
        foreach (RefIndexValue iv in row)
        {
            result.Add(iv.Value);
        }
        return result;
    }
}