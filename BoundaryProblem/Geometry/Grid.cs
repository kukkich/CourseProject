namespace BoundaryProblem.Geometry
{
    public class Grid
    {
        public List<Point2D> Nodes { get; private set; }
        public List<Element> Elements { get; private set; }

        public IEnumerable<NodeIndexes> ElementNodeIndexes
        {
            get
            {
                var nodesPerXAxis = _splitParameter.XSteps + 1;
                for (var i = 0; i < _splitParameter.YSteps; i++)
                {
                    var leftBottomStartIndex = i * nodesPerXAxis;
                    
                    var leftTopStartIndex = leftBottomStartIndex + nodesPerXAxis;

                    for (var j = 0; j < _splitParameter.XSteps; j++)
                    {
                        var leftBottomIndex = leftBottomStartIndex + j;
                        var leftTopIndex = leftTopStartIndex + j;

                        yield return new NodeIndexes(
                            leftBottomIndex,
                            leftBottomIndex + 1,
                            leftTopIndex,
                            leftTopIndex + 1
                            );
                    }
                }
            }
        }

        private readonly AxisSplitParameter _splitParameter;


        public Grid(IEnumerable<Point2D> nodes, IEnumerable<Element> elements, AxisSplitParameter splitParameter)
        {
            _splitParameter = splitParameter;
            Nodes = nodes?.ToList();
            Elements = elements?.ToList();
        }
    }
}
