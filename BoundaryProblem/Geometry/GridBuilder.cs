namespace BoundaryProblem.Geometry
{
    public class GridBuilder
    {
        private readonly Rectangle _area;
        private AxisSplitParameter _splitParameter;
        private Point2D _stepSize;
        public GridBuilder(Rectangle area)
        {
            _area = area;
        }

        public Grid Build(AxisSplitParameter splitParameter)
        {
            _splitParameter = splitParameter;
            _stepSize = new Point2D(
                CalcStep(_area.LeftBottom.X, _area.RightBottom.X, _splitParameter.XSteps),
                CalcStep(_area.LeftBottom.Y, _area.LeftTop.Y, _splitParameter.YSteps)
            );
            

            return new Grid(
                GenerateNodes(),
                GenerateElements(),
                _splitParameter
                );
        }

        public IEnumerable<Point2D> GetNodes(AxisSplitParameter splitParameter)
        {
            _stepSize = new Point2D(
                CalcStep(_area.LeftBottom.X, _area.RightBottom.X, _splitParameter.XSteps),
                CalcStep(_area.LeftBottom.Y, _area.LeftTop.Y, _splitParameter.YSteps)
            );
            return GenerateNodes();
        }

        private static double CalcStep(double loweBound, double upperBound, int stepsCount)
        {
            return (upperBound - loweBound) / stepsCount;
        }

        private IEnumerable<Point2D> GenerateNodes()
        {
            for (var i = 0; i <= _splitParameter.YSteps; i++)
            {
                for (var j = 0; j <= _splitParameter.XSteps; j++)
                {
                    var x = _area.LeftBottom.X + _stepSize.X * j;
                    var y = _area.LeftBottom.Y + _stepSize.Y * i;

                    yield return new Point2D(x, y);
                }
            }
        }

        private IEnumerable<Element> GenerateElements()
        {
            for (var i = 0; i < _splitParameter.YSteps; i++)
            {
                for (var j = 0; j < _splitParameter.XSteps; j++)
                {
                    var x = _area.LeftBottom.X + _stepSize.X * j;
                    var y = _area.LeftBottom.Y + _stepSize.Y * i;

                    yield return new Element(new Rectangle(
                        new Point2D(x, y),
                        new Point2D(x + _stepSize.X, y),
                        new Point2D(x, y + _stepSize.Y),
                        new Point2D(x + _stepSize.X, y + _stepSize.Y)
                    ));
                }
            }
        }
    }
}
