using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.DataStructures.DensityFunction;
using BoundaryProblem.Geometry;
using BoundaryProblem.Time.Splitting;

namespace BoundaryProblem.Time;

public class SolutionContext
{
    public TimeSeries TimeSeries { get; set; }
    public int Index => TimeSeries.Index;
    public Vector[] Solutions { get; set; }
    public Grid Grid { get; init; }
    public FunctionDependentDensity F { get; set; }
}