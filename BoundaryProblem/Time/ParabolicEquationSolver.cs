using BoundaryProblem.Calculus.Equation.DataStructures;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.DensityFunction;
using BoundaryProblem.Geometry;
using BoundaryProblem.Loggining;
using BoundaryProblem.Time.Boundary;
using BoundaryProblem.Time.Splitting;

namespace BoundaryProblem.Time;

public class ParabolicEquationSolver
{
    private readonly FirstTimeBoundary _firstBoundary;
    public FiniteElementSolver EllipticSolver { get; }
    public TimeSeries Time { get; }
    public ProblemFilePathsProvider Files => EllipticSolver.Files;
    private readonly MaterialProvider _materials;
    private readonly SolutionContext _context;

    public ParabolicEquationSolver(
        Func<TimePoint2D, double> u0, 
        Func<TimePoint2D, double> f, 
        FiniteElementSolver ellipticSolver,
        TimeSeries time,
        FirstTimeBoundary firstBoundary
        )
    {
        _firstBoundary = firstBoundary;
        EllipticSolver = ellipticSolver;
        Grid grid = ellipticSolver.Grid;

        _materials = MaterialProvider.Deserialize(Files);
        ellipticSolver.Materials = _materials;
        Time = time;

        Vector[] solutions = new Vector[Time.Values.Length];
        for (int i = 0; i < 3; i++)
        {
            solutions[i] = Vector.Create(grid.Nodes.Length, index =>
            {
                Point2D p = grid.Nodes[index];
                return u0(new TimePoint2D(p.X, p.Y, Time[i]));
            });
        }
        for (int i = 3; i < solutions.Length; i++)
        {
            solutions[i] = Vector.Create(grid.Nodes.Length);
        }
        Time.Index = 3;

        _context = new SolutionContext
        {
            Grid = grid,
            Solutions = solutions,
            TimeSeries = Time
        };
        _context.F = new FunctionDependentDensity(f, _context);
    }

    public void IterateToEnd()
    {
        for (; Time.Index < Time.Values.Length;)
        {
            Iterate();
        }
    }

    public void Iterate()
    {
        Console.WriteLine($"{_context.Index}: {_context.TimeSeries.CurrentTime:F3} ");
        WriteF(_context);
        WriteBoundaries();
        SetGamma();
        EllipticSolver.ReadData();

        var solution = EllipticSolver.Solve();
        _context.Solutions[_context.Index] = solution.FunctionWeights;

        _context.TimeSeries.Index++;
    }

    public FiniteElementSolution GetSolution(double time)
    {
        var index = Array.BinarySearch(Time.Values, time);
        return new FiniteElementSolution(_context.Grid, _context.Solutions[index]);
    }

    private void WriteF(SolutionContext context)
    {
        
        var grid = context.Grid;
        var f = context.F;
        var material = _materials.GetMaterialById(grid.Elements.First().MaterialId);

        using var writer = new StreamWriter(Files.DensityFunction);
        writer.WriteLine(grid.Nodes.Length);
        var solutions = _context.Solutions.ToArray();
        var u3 = solutions[_context.Index - 3];
        var u2 = solutions[_context.Index - 2];
        var u1 = solutions[_context.Index - 1];

        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            var value = f.Calc(i);
            GetTimeCoefficients(out var n1, out var n2, out var n3);

            value -= material.Sigma * (u3[i]*n3 + u2[i]*n2 + u1[i]*n1);

            writer.WriteLine($"{i} {value:F15}");
        }
    }

    private void GetTimeCoefficients(out double n1, out double n2, out double n3)
    {
        Func<int, int, double> d = _context.TimeSeries.Delta;
        Func<int, double> delta = _context.TimeSeries.Delta;
        n3 = delta(2) * delta(1) / (d(3, 2) * d(3, 1) * d(3, 0));
        n2 = delta(3) * delta(1) / (d(2, 3) * d(2, 1) * d(2, 0));
        n1 = delta(3) * delta(2) / (d(1, 3) * d(1, 2) * d(1, 0));
    }

    private void WriteBoundaries()
    {
        _firstBoundary.Write(_context, Files);
    }

    private void SetGamma()
    {
        Func<int, int, double> d = _context.TimeSeries.Delta;
        Func<int, double> delta = _context.TimeSeries.Delta;

        var timeImpact = delta(2)*delta(1) + delta(3)*delta(1) + delta(3)*delta(2);
        timeImpact /= delta(3) * delta(2) * delta(1);
        
        for (int i = 0; i < _materials.Materials.Length; i++)
        {
            var material = _materials.Materials[i];
            _materials.Materials[i] = material with
            {
                Gamma = material.Sigma * timeImpact
            };
        }
    }
    
}