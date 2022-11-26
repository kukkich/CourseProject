using BoundaryProblem.DataStructures;

namespace BoundaryProblem.Geometry
{
    public class Grid
    {
        public IEnumerable<Point2D> Nodes { get; }
        public Element[] Elements { get; }
        public Point2D ElementLength { get; }
        public Point2D DistanceBetweenNodes => ElementLength / Element.StepsInsideElement;

        public Grid(
            IEnumerable<Point2D> nodes,
            IEnumerable<Element> elements,
            Point2D elementLength
        )
        {
            Nodes = nodes.ToArray();
            Elements = elements.ToArray();
            ElementLength = elementLength;
        }

        public static void Serialize(Grid grid, ProblemFilePathsProvider filesProvider)
        {
            using StreamWriter nodesWriter = new(filesProvider.Nodes);
            SerializeNodes(grid.Nodes, nodesWriter);

            using StreamWriter elementsWriter = new(filesProvider.Elems);
            SerializeElems(grid.Elements, elementsWriter);
        }

        private static void SerializeNodes(IEnumerable<Point2D> nodes, TextWriter writer)
        {
            writer.WriteLine(nodes.Count());
            foreach (var (x, y) in nodes)
                writer.WriteLine(x + ' ' + y);
        }

        private static void SerializeElems(IEnumerable<Element> elems, TextWriter writer)
        {
            writer.WriteLine(elems.Count());
            foreach (var elem in elems)
            {
                foreach (var index in elem.NodeIndexes)
                {
                    writer.Write(index);
                    writer.Write(' ');
                }
                writer.Write(elem.MaterialId);
                writer.WriteLine();
            }
        }
    }
}
