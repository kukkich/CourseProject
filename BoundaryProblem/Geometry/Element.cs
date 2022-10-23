namespace BoundaryProblem.Geometry
{
    public readonly record struct Element
    {
        public IEnumerable<Point2D> LocalNodes
        {
            get
            {
                yield return _rectangle.LeftBottom;
                yield return _rectangle.RightBottom;
                yield return _rectangle.LeftTop;
                yield return _rectangle.RightTop;
            }
        }

        private readonly Rectangle _rectangle;

        public Element(Rectangle rectangle)
        {
            _rectangle = rectangle;
        }
    }
}
