namespace BoundaryProblem.Geometry
{
    public interface IFiniteElement<out TPoints>
    {
        public IEnumerable<TPoints> LocalNodes { get; }
    }
}
