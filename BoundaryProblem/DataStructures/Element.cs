using BoundaryProblem.Geometry;

namespace BoundaryProblem.DataStructures
{
    public readonly record struct Element
    {
        public const int StepsInsideElement = 3;
        public const int NodesInElement = (StepsInsideElement + 1) * (StepsInsideElement + 1);
        public int[] NodeIndexes { get; }

        public readonly int MaterialId;

        // TODO Remove this
        private readonly Rectangle _rectangle;

        public Element(Rectangle rectangle, int[] nodeIndexes, int materialId = 0)
        {
            _rectangle = rectangle;
            NodeIndexes = nodeIndexes;
            MaterialId = materialId;
        }
    }
}
