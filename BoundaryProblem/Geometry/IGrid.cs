namespace BoundaryProblem.Geometry
{
    public interface IGrid<out TPoints, out TElements>
        where TElements : IFiniteElement<TPoints>
    {
        public IEnumerable<TElements> Elements { get; }
        public IEnumerable<TPoints> Nodes { get; }
    }
}
