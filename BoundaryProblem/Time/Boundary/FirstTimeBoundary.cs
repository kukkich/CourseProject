using BoundaryProblem.DataStructures;
using BoundaryProblem.Geometry;

namespace BoundaryProblem.Time.Boundary;

public class FirstTimeBoundary : Dictionary<int, Bound[]>
{
    public Func<TimePoint2D, double> U { get; }
    public FirstTimeBoundary(Func<TimePoint2D, double> u)
    {
        U = u;
    }

    public void Write(SolutionContext context, ProblemFilePathsProvider files)
    {
        using var writer = new StreamWriter(files.FirstBoundary);
        foreach (var elemIndex in Keys)
        {
            var elem = context.Grid.Elements[elemIndex];
            foreach (var bound in this[elemIndex])
            {
                var boundNodes = elem.GetBoundNodeIndexes(bound);
                foreach (var nodeIndex in boundNodes)
                {
                    var p = context.Grid.Nodes[nodeIndex];
                    var value = U(new TimePoint2D(p.X, p.Y, context.TimeSeries.CurrentTime));
                    writer.WriteLine($"{nodeIndex} {value:F15}");
                }
            }
        }
    }
}