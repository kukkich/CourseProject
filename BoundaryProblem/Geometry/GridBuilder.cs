namespace BoundaryProblem.Geometry
{
    public class GridBuilder
    {
        private readonly Rectangle _area;

        public GridBuilder(Rectangle area)
        {
            _area = area;
        }

        public Grid Build(int stepsCount)
        {
            var xStep = CalcStep(_area.LeftBottom.X, _area.RightBottom.X, stepsCount);
            var yStep = CalcStep(_area.LeftBottom.Y, _area.LeftTop.Y, stepsCount);

            return new Grid(
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

        private IEnumerable<Element> GenerateElements(double xStep, double yStep, int stepsCount)
        {
            for (var i = 0; i < stepsCount; i++)
            {
                for (var j = 0; j < stepsCount; j++)
                {
                    var x = _area.LeftBottom.X + xStep * j;
                    var y = _area.LeftBottom.Y + yStep * i;
                    yield return new Element(new Rectangle(
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
