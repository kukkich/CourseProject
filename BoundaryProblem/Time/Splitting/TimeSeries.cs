namespace BoundaryProblem.Time.Splitting;

public class TimeSeries
{
    public double this[int index] => Values[index];
    public double[] Values { get; }
    public int Index { get; set; }
    public double CurrentTime => Values[Index];

    public TimeSeries(UniformSplitter splitter, Interval interval)
    {
        Values = splitter
            .EnumerateValues(interval)
            .ToArray();
    }

    /// <returns>t[j - p] - t[j - p]</returns>
    public double Delta(int p, int q)
    {
        if (Math.Abs(p - q) > 3) throw new InvalidOperationException();

        return Values[Index - p] - Values[Index - q];
    }

    /// <returns>t[j] - t[j - k]</returns>
    public double Delta(int k)
    {
        if (k > 3) throw new InvalidOperationException();

        return Values[Index] - Values[Index - k];
    }
}