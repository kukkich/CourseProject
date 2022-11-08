namespace BoundaryProblem.DataStructures
{
    public readonly record struct Element(int[] NodeIndexes, int MaterialId = 0)
    {
        public const int StepsInsideElement = 3;
        public const int NodesInElement = (StepsInsideElement + 1) * (StepsInsideElement + 1);

        public readonly int MaterialId = MaterialId;
    }
}
