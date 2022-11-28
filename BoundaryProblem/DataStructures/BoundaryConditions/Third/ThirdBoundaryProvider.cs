namespace BoundaryProblem.DataStructures.BoundaryConditions.Third;

public record ThirdBoundaryProvider(FlowExchangeUnit[] FlowExchangeConditions)
{
    public static ThirdBoundaryProvider Serialize()
    {
        throw new NotImplementedException();
    }
}