namespace BoundaryProblem.DataStructures.DensityFunction;

public class FunctionDefinedOnNodes : IDensityFunctionProvider
{
    private readonly double[] _values;

    public FunctionDefinedOnNodes(double[] values)
    {
        _values = values;
    }

    public static IDensityFunctionProvider Deserialize(ProblemFilePathsProvider files)
    {
        using var stream = new StreamReader(files.DensityFunction);
        var line = stream.ReadLine();
        var size = int.Parse(line);
        if (String.IsNullOrEmpty(line)) throw new Exception("No F size");
        var functionValues = new double[size];

        while (true)
        {
            line = stream.ReadLine();
            if (String.IsNullOrEmpty(line)) break;

            var values = line.Split(' ');
            functionValues[int.Parse(values[0])] = double.Parse(values[1]);
        }

        return new FunctionDefinedOnNodes(functionValues);
    }

    public double Calc(int globalNodeIndex)
    {
            return _values[globalNodeIndex];

        throw new ArgumentOutOfRangeException($"There no node with index {globalNodeIndex}");
    }
}