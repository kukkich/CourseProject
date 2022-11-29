namespace BoundaryProblem.DataStructures.BoundaryConditions.Third;

public record ThirdBoundaryProvider(FlowExchangeUnit[] FlowExchangeConditions)
{
    public static ThirdBoundaryProvider Deserialize(ProblemFilePathsProvider filesProvider)
    {
        using var stream = new StreamReader(filesProvider.ThirdBoundary);
        List<FlowExchangeUnit> units = new();

        while (true)
        {
            var line = stream.ReadLine();
            if (String.IsNullOrEmpty(line)) break;

            var values = line.Split(' ');
            if (Enum.GetValues<Bound>()
                .Select(x => (int)x)
                .All(x => x != int.Parse(values[1]))
                )
            {
                throw new FormatException(
                    "Third boundary file contains wrong bound index.\n" +
                    $"String was: \"{line}\""
                    );
            }

            units.Add(
                new FlowExchangeUnit(
                    ElementId: int.Parse(values[0]),
                    Bound: (Bound)int.Parse(values[1]),
                    Betta: double.Parse(values[2]),
                    Environment: double.Parse(values[3])
                ));
        }

        return new ThirdBoundaryProvider(units.ToArray());
    }
}