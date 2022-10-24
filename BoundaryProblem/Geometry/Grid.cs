namespace BoundaryProblem.Geometry
{
    public class Grid
    {
        public Point2D[] Nodes { get; private set; }
        public Element[] Elements { get; private set; }
        public NodeIndexes[] ElementNodeIndexes { get; private set; }

        public Grid(
            IEnumerable<Point2D> nodes,
            IEnumerable<Element> elements,
            IEnumerable<NodeIndexes> elementNodeIndexes
        )
        {
            ElementNodeIndexes = elementNodeIndexes.ToArray();
            Nodes = nodes.ToArray();
            Elements = elements.ToArray();
        }
    }
}
