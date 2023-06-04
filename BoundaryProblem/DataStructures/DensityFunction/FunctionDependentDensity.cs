using BoundaryProblem.Geometry;
using BoundaryProblem.Time;

namespace BoundaryProblem.DataStructures.DensityFunction;

public class FunctionDependentDensity : IDensityFunctionProvider 
{
    private readonly Func<TimePoint2D, double> _f;
    private readonly SolutionContext _context;

    public FunctionDependentDensity(Func<TimePoint2D, double> f, SolutionContext context)
    {
        _f = f;
        _context = context;
    }

    public double Calc(int globalNodeIndex)
    {
        Point2D p = _context.Grid.Nodes[globalNodeIndex];
        return _f(new TimePoint2D(p.X, p.Y, _context.TimeSeries.CurrentTime));
    }
}