namespace BoundaryProblem.DataStructures.BoundaryConditions.Third;

public record ThirdBoundaryProvider(
    FlowUnit[] FlowConditions,
    BoundaryValueUnit[] EnvironmentValues
    )
{
    public static ThirdBoundaryProvider Serialize()
    {
        throw new NotImplementedException();
    }
}