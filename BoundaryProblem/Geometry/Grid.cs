namespace BoundaryProblem.Geometry
{
    public class Grid
    {
        public IEnumerable<Point2D> Nodes { get; }
        public IEnumerable<Element> Elements { get; }

        public Grid(
            IEnumerable<Point2D> nodes,
            IEnumerable<Element> elements
        )
        {
            Nodes = nodes.ToArray();
            Elements = elements.ToArray();
        }

        public void Serialize(ProblemFilePathsProvider filesProvider)
        {
            using StreamWriter nodesWriter = new(filesProvider.Nodes);
            SerializeNodes(nodesWriter);

            using StreamWriter elementsWriter = new(filesProvider.Elems);
            SerializeElems(elementsWriter);
        }

        private void SerializeNodes(TextWriter writer)
        {
            writer.WriteLine(Nodes.Count());
            foreach (var (x, y) in Nodes)
                writer.WriteLine(x + ' ' + y);
        }

        private void SerializeElems(TextWriter writer)
        {
            writer.WriteLine(Elements.Count());
            foreach (var elem in Elements)
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
