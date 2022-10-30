namespace BoundaryProblem
{
    public class ProblemFilePathsProvider
    {
        public string Nodes
        {
            get => _rootDirectory + _nodes;
            set => _nodes = value;
        }
        private string _nodes;

        public string Elems
        {
            get => _rootDirectory + _elems;
            set => _elems = value;
        }
        private string _elems;

        private readonly string _rootDirectory;

        public ProblemFilePathsProvider(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }
    }
}
