namespace BoundaryProblem.Geometry
{
    public class Grid
    {
        public List<Point2D> Nodes { get; private set; }
        public List<Element> Elements { get; private set; }
        public List<NodeIndexes> ElementNodeIndexes { get; private set; }

        public Grid(
            IEnumerable<Point2D> nodes, 
            IEnumerable<Element> elements, 
            IEnumerable<NodeIndexes> elementNodeIndexes
        )
        {
            ElementNodeIndexes = elementNodeIndexes.ToList();
            Nodes = nodes.ToList();
            Elements = elements.ToList();
        }
    }
}
