namespace BoundaryProblem.DataStructures.BoundaryConditions.Second;

public record SecondBoundaryProvider(params FlowUnit[] FlowConditions)
{
    public static SecondBoundaryProvider Serialize()
    {
        throw new NotImplementedException();
    }
}