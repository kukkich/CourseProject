namespace BoundaryProblem.Geometry
{
    public class Grid
    {
        public IEnumerable<Point2D> Nodes { get; private set; }
        public IEnumerable<Element> Elements { get; private set; }

        public Grid(IEnumerable<Point2D> nodes, IEnumerable<Element> elements)
        {
            Nodes = nodes.ToList();
            Elements = elements.ToList();
        }
    }
}
