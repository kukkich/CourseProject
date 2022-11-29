namespace BoundaryProblem.DataStructures.BoundaryConditions.First;

public record FirstBoundaryProvider(params ValueUnit[] ValueConditions)
{
    public static FirstBoundaryProvider Deserialize()
    {
        throw new NotImplementedException();
    }
}