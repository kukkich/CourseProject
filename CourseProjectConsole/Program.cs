using BoundaryProblem;
using BoundaryProblem.Geometry;
using BoundaryProblem.Loggining;

namespace CourseProjectConsole;

public static class Program
{
    private const string Root = "C:\\Users\\vitia\\OneDrive\\Рабочий стол\\FiniteElems\\Тесты\\1322U=x^2+2x (1,0)\\";
    public static Logger Logger { get; set; } = new Logger();

    private static void Main()
    {
        var files = new ProblemFilePathsProvider(Root)
        {
            Elems = "elems.txt",
            Nodes = "nodes.txt",
            FirstBoundary = "bar1.txt",
            SecondBoundary = "bar2.txt",
            ThirdBoundary = "bar3.txt",
            DensityFunction = "f.txt",
            Material = "mat.txt"
        };

        var grid = MakeGrid(files);
        MakeF(grid, files, p => -2);

        var solver = new FiniteElementSolver(files)
        {
            MaxIteration = 1000,
            SolutionPrecision = 1e-14
        };
        var solution = solver.Solve();

        Logger.LogVector(solution.FunctionWeights);

        Console.WriteLine();
        for (double y = 6; y >= 0; y -= 2d)
        {
            Console.Write($"{y:F5}: ");
            for (double x = 0d; x <= 3d; x += 1d)
            {
                Console.Write($"{solution.Calculate(new Point2D(x, y)):F5}   ");
            }
            Console.WriteLine();
        }
        Console.Write($"         ");
        for (double x = 0d; x <= 3d; x += 1d)
        {
            Console.Write($"{x:F5}   ");
        }
    }

    private static Grid MakeGrid(ProblemFilePathsProvider files)
    {
        var gridBuilder = new GridBuilder(new Rectangle(
            LeftBottom: new Point2D(0, 0),
            RightBottom: new Point2D(3, 0),
            LeftTop: new Point2D(0, 6),
            RightTop: new Point2D(3, 6)
        ));

        Grid grid = gridBuilder.Build(new AxisSplitParameter(XSteps: 1, YSteps: 1));
        Grid.Serialize(grid, files);

        return grid;
    }

    private static void MakeF(Grid grid, ProblemFilePathsProvider files, Func<Point2D, double> f)
    {
        using var writer = new StreamWriter(files.DensityFunction);

        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            var node = grid.Nodes[i];
            var value = f(node);
            writer.WriteLine($"{i} {value:F15}");
        }
    }
}
