using BoundaryProblem.Geometry;

namespace BoundaryProblem.DataStructures
{
    public readonly record struct Element
    {
        public int[] NodeIndexes { get; }

        public const int STEPS_INSIDE_ELEMENT = 3;

        public readonly int MaterialId;

        private readonly Rectangle _rectangle;

        public Element(Rectangle rectangle, int[] nodeIndexes, int materialId = 0)
        {
            _rectangle = rectangle;
            NodeIndexes = nodeIndexes;
            MaterialId = materialId;
        }
    }
}
