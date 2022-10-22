namespace BoundaryProblem.Geometry
{
    public interface IComputationalArea<TPoints, TElements>
        where TElements : IFiniteElement<TPoints>
    {
        public IGrid<TPoints, TElements> BuildGrid(int stepsCount);
    }
}
