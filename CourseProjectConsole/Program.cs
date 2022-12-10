using BoundaryProblem;
using BoundaryProblem.DataStructures;
using BoundaryProblem.DataStructures.BoundaryConditions.First;
using BoundaryProblem.DataStructures.BoundaryConditions.Second;
using BoundaryProblem.DataStructures.BoundaryConditions.Third;
using BoundaryProblem.DataStructures.DensityFunction;
using BoundaryProblem.Geometry;

namespace CourseProjectConsole;

internal class Program
{
    private const string Root = "C:\\Users\\vitia\\OneDrive\\Рабочий стол\\123x\\";
    
    private static void Main()
    {
        Deserializing();

        Console.ReadLine();
    }

    private static void Deserializing()
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

        var first = FirstBoundaryProvider.Deserialize(files);
        var second = SecondBoundaryProvider.Deserialize(files);
        var third = ThirdBoundaryProvider.Deserialize(files);

        var gridBuilder = new GridBuilder(new Rectangle(
            LeftBottom: new Point2D(0, 0),
            RightBottom: new Point2D(3, 0),
            LeftTop: new Point2D(0, 10),
            RightTop: new Point2D(3, 10)
        ));

        Grid grid = gridBuilder.Build(new AxisSplitParameter(XSteps: 3, YSteps: 1));
        Grid.Serialize(grid, files);
        var newGrid = Grid.Deserialize(files);

        IDensityFunctionProvider f = FunctionDefinedOnNodes.Deserialize(files);
        IMaterialProvider materials = MaterialProvider.Deserialize(files);
    }
}
