namespace BoundaryProblem.Geometry
{
    public class RectGrid : IGrid<Point2D, RectElement>
    {
        public IEnumerable<Point2D> Nodes { get; private set; }
        public IEnumerable<RectElement> Elements { get; private set; }

        public RectGrid(IEnumerable<Point2D> nodes, IEnumerable<RectElement> elements)
        {
            Nodes = nodes;
            Elements = elements;
        }
    }
}
