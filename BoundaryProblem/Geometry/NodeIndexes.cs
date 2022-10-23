namespace BoundaryProblem.Geometry
{
    /// <summary>
    /// --------
    /// |3    4|
    /// |      |
    /// |1    2|
    /// --------
    /// </summary>
    public readonly record struct NodeIndexes(
        int LeftBottom,
        int RightBottom,
        int LeftTop,
        int RightTop
    );
}
