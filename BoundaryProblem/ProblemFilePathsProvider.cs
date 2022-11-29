namespace BoundaryProblem;

public class ProblemFilePathsProvider
{
    public string Nodes
    {
        get => AttachRootPath(_nodes);
        set => _nodes = value;
    }
    private string _nodes;
    public string Elems
    {
        get => AttachRootPath(_elems);
        set => _elems = value;
    }
    private string _elems;
    public string FirstBoundary
    {
        get => AttachRootPath(_firstBoundary);
        set => _firstBoundary = value;
    }
    private string _firstBoundary;
    public string SecondBoundary
    {
        get => AttachRootPath(_secondBoundary);
        set => _secondBoundary = value;
    }
    private string _secondBoundary;
    public string ThirdBoundary
    {
        get => AttachRootPath(_thirdBoundary);
        set => _thirdBoundary = value;
    }
    private string _thirdBoundary;

    private readonly string _rootDirectory;

    public ProblemFilePathsProvider(string rootDirectory)
    {
        _rootDirectory = rootDirectory;
    }

    private string AttachRootPath(string fileName)
    {
        return _rootDirectory + fileName;
    }
}