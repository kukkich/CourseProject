namespace BoundaryProblem.Geometry
{
    public class RectangleArea : IComputationalArea<Point2D, RectElement>
    {
        private readonly Rectangle _area;

        public RectangleArea(Rectangle area)
        {
            _area = area;
        }

        public IGrid<Point2D, RectElement> BuildGrid(int stepsCount)
        {
            var xStep = CalcStep(_area.LeftBottom.X, _area.RightBottom.X, stepsCount);
            var yStep = CalcStep(_area.LeftBottom.Y, _area.LeftTop.Y, stepsCount);

            return new RectGrid(
                GenerateNodes(xStep, yStep, stepsCount),
                GenerateElements(xStep, yStep, stepsCount)
                );
        }

        private static double CalcStep(double loweBound, double upperBound, int stepsCount)
        {
            return (upperBound - loweBound) / stepsCount;
        }

        private IEnumerable<Point2D> GenerateNodes(double xStep, double yStep, int stepsCount)
        {
            for (var i = 0; i <= stepsCount; i++)
            {
                for (var j = 0; j <= stepsCount; j++)
                {
                    var x = _area.LeftBottom.X + xStep * j;
                    var y = _area.LeftBottom.Y + yStep * i;

                    yield return new Point2D(x, y);
                }
            }

        }

        public IEnumerable<RectElement> GenerateElements(double xStep, double yStep, int stepsCount)
        {
            for (var i = 0; i < stepsCount; i++)
            {
                for (var j = 0; j < stepsCount; j++)
                {
                    var x = _area.LeftBottom.X + xStep * j;
                    var y = _area.LeftBottom.Y + yStep * i;
                    yield return new RectElement(new Rectangle(
                        new Point2D(x, y),
                        new Point2D(x + xStep, y),
                        new Point2D(x, y + yStep),
                        new Point2D(x + xStep, y + yStep)
                    ));
                }
            }
        }
    }
}
