using System.Collections;

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
    ) : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            yield return LeftBottom;
            yield return RightBottom;
            yield return LeftTop;
            yield return RightTop;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
