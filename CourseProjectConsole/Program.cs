﻿using System.Globalization;
using BoundaryProblem;
using BoundaryProblem.DataStructures;
using BoundaryProblem.Geometry;
using BoundaryProblem.Loggining;
using BoundaryProblem.Time;
using BoundaryProblem.Time.Boundary;
using BoundaryProblem.Time.Splitting;

namespace CourseProjectConsole;

public static class Program
{
    const string Root = "C:\\Users\\vitia\\OneDrive\\Рабочий стол\\FiniteElems\\Time\\Simple\\";

    public static Logger Logger { get; set; } = new Logger();

    public static Point2D Min = new (0, 0);
    public static Point2D Max = new (2, 2);
    public static Point2D Step = new (1d / N, 1d / N);
    public const int N = 2;
    public const double Lambda = 0.5d;
    public const double Gamma = 2d;

    private static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

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
        Func<TimePoint2D, double> u = (p) => (Math.Pow(p.X, 3) + Math.Pow(p.Y, 3)) * Math.Pow(p.T, 3);
        Func<TimePoint2D, double> f = (p) => -6d * (Math.Pow(p.X, 1) + Math.Pow(p.Y, 1)) * Math.Pow(p.T, 3) + 3d* Math.Pow(p.T, 2)* (Math.Pow(p.X, 3) + Math.Pow(p.Y, 3));

        var grid = MakeGrid(files);

        var solver = new ParabolicEquationSolver(u, f,
            ellipticSolver: new FiniteElementSolver(files)
            {
                MaxIteration = 1000,
                SolutionPrecision = 1e-14
            },
            time: new TimeSeries(new UniformSplitter(30), new Interval(0, 3)),
            new FirstTimeBoundary(u)
            {
                [0] = new[] { Bound.Bottom, Bound.Left },
                [1] = new[] { Bound.Bottom, Bound.Right },
                [2] = new[] { Bound.Top, Bound.Left },
                [3] = new[] { Bound.Top, Bound.Right },
            }
        );
        solver.IterateToEnd();
        var solution = solver.GetSolution(3d);
        Console.WriteLine($"{solution.Calculate(new Point2D(1d/3, 4d/3))} {u(new TimePoint2D(1d/3, 4d/3, 3d))}");
    }

    private static void SolveExample(Grid grid, ProblemFilePathsProvider files)
    {
        MakeF(
            grid, files,
            p => -1d * Lambda * Math.Exp(p.X * p.Y) * (p.Y * p.Y + p.X * p.X - Gamma / Lambda)
        );
        MakeFirstBoundaryFor2X2(grid, files);

        var solver = new FiniteElementSolver(files)
        {
            MaxIteration = 1000,
            SolutionPrecision = 1e-14
        };
        var solution = solver.Solve();

        Logger.LogVector(solution.FunctionWeights);

        Console.WriteLine();
        for (double y = Max.Y; y >= Min.Y; y -= Step.Y)
        {
            Console.Write($"{y:F5}: ");
            for (double x = Min.X; x <= Max.X; x += Step.X)
            {
                Console.Write($"{solution.Calculate(new Point2D(x, y)):F5}   ");
            }

            Console.WriteLine();
        }

        Console.Write($"        ");
        for (double x = Min.X; x <= Max.X; x += Step.X)
        {
            Console.Write($"{x:F5}   ");
        }

        //Console.WriteLine();
        //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        //Console.WriteLine($"U(0.35, 2.65) = {solution.Calculate(new Point2D(0.35, 2.65)):E15}");
        //Console.WriteLine($"U(-1.4, 2.15) = {solution.Calculate(new Point2D(-1.4, 2.15)):E15}");
    }

    private static Grid MakeGrid(ProblemFilePathsProvider files)
    {
        var gridBuilder = new GridBuilder(new Rectangle(
            LeftBottom: new Point2D(Min.X, Min.Y),
            RightBottom: new Point2D(Max.X, Min.Y),
            LeftTop: new Point2D(Min.X, Max.Y),
            RightTop: new Point2D(Max.X, Max.Y)
        ));

        Grid grid = gridBuilder.Build(new AxisSplitParameter(XSteps: N, YSteps: N));
        Grid.Serialize(grid, files);

        return grid;
    }

    private static void MakeF(Grid grid, ProblemFilePathsProvider files, Func<Point2D, double> f)
    {
        using var writer = new StreamWriter(files.DensityFunction);
        writer.WriteLine(grid.Nodes.Length);
        for (var i = 0; i < grid.Nodes.Length; i++)
        {
            var node = grid.Nodes[i];
            var value = f(node);
            writer.WriteLine($"{i} {value:F15}");
        }
    }

    public static void MakeFirstBoundary(int[] elemIndexes, Bound[] bounds, Func<Point2D, double> u, Grid grid, ProblemFilePathsProvider files)
    {
        using var writer = new StreamWriter(files.FirstBoundary);
        for (int i = 0; i < elemIndexes.Length; i++)
        {
            var elem = grid.Elements[elemIndexes[i]];
            var boundNodes = elem.GetBoundNodeIndexes(bounds[i]);
            foreach (var nodeIndex in boundNodes)
            {
                writer.WriteLine($"{nodeIndex} {u(grid.Nodes[nodeIndex]):F15}");
            }
        }
    }

    #region 1 FirstTimeBoundary
    public static void MakeFirstBoundaryFor2X2(Grid grid, ProblemFilePathsProvider files)
    {
        MakeFirstBoundary(
            new[]
            {
                0, 0,
                1, 1,
                2, 2,
                3, 3
            },
            new[]
            {
                Bound.Bottom, Bound.Left,
                Bound.Bottom, Bound.Right,
                Bound.Left, Bound.Top,
                Bound.Top, Bound.Right
            },
            p => Math.Exp(p.X * p.Y),
            grid,
            files
        );
    }

    public static void MakeFirstBoundaryFor4X4(Grid grid, ProblemFilePathsProvider files)
    {
        MakeFirstBoundary(
            new[]
            {
                0, 0,
                1,
                2,
                3, 3,

                4, 7,
                8, 11,

                12, 12,
                13,
                14,
                15, 15

            },
            new[]
            {
                Bound.Bottom, Bound.Left,
                Bound.Bottom,
                Bound.Bottom,
                Bound.Bottom, Bound.Right,

                Bound.Left, Bound.Right,
                Bound.Left, Bound.Right,

                Bound.Top, Bound.Left,
                Bound.Top,
                Bound.Top,
                Bound.Top, Bound.Right
            },
            p => Math.Exp(p.X * p.Y),
            grid,
            files
        );
    }

    public static void MakeFirstBoundaryFor8X8(Grid grid, ProblemFilePathsProvider files)
    {
        List<int> elemIndexes = new List<int>();
        List<Bound> bounds = new List<Bound>();
        for (int i = 0; i <= 7; i++)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Bottom);
        }
        for (int i = 56; i <= 63; i++)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Top);
        }
        for (int i = 0; i <= 56; i += 8)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Left);
        }
        for (int i = 7; i <= 63; i += 8)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Right);
        }
        MakeFirstBoundary(
            elemIndexes.ToArray(),
            bounds.ToArray(),
            p => Math.Exp(p.X * p.Y),
            grid,
            files
        );
    }

    public static void MakeFirstBoundaryFor16X16(Grid grid, ProblemFilePathsProvider files)
    {
        List<int> elemIndexes = new List<int>();
        List<Bound> bounds = new List<Bound>();
        for (int i = 0; i <= 15; i++)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Bottom);
        }
        for (int i = 240; i <= 255; i++)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Top);
        }
        for (int i = 0; i <= 240; i += 16)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Left);
        }
        for (int i = 15; i <= 255; i += 16)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Right);
        }
        MakeFirstBoundary(
            elemIndexes.ToArray(),
            bounds.ToArray(),
            p => Math.Exp(p.X * p.Y),
            grid,
            files
        );
    }

    public static void MakeFirstBoundaryFor32X32(Grid grid, ProblemFilePathsProvider files)
    {
        List<int> elemIndexes = new List<int>();
        List<Bound> bounds = new List<Bound>();

        for (int i = 0; i <= 31; i++)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Bottom);
        }
        for (int i = 992; i <= 1023; i++)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Top);
        }
        for (int i = 0; i <= 992; i += 32)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Left);
        }
        for (int i = 31; i <= 1023; i += 32)
        {
            elemIndexes.Add(i);
            bounds.Add(Bound.Right);
        }
        MakeFirstBoundary(
            elemIndexes.ToArray(),
            bounds.ToArray(),
            p => Math.Exp(p.X * p.Y),
            grid,
            files
        );
    }
    #endregion
}
