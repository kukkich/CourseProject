namespace BoundaryProblem.Geometry
{
    /// <summary>
    /// --------
    /// |3    4|
    /// |      |
    /// |1    2|
    /// --------
    /// </summary>
    public readonly record struct Rectangle(
        Point2D LeftBottom,
        Point2D RightBottom,
        Point2D LeftTop,
        Point2D RightTop
        );
}
