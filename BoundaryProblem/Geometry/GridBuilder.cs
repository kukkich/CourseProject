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
                GenerateNodes,
                GenerateElements,
                ElementNodeIndexes
                );
        }

        private static double CalcStep(double loweBound, double upperBound, int stepsCount)
        {
            return (upperBound - loweBound) / stepsCount;
        }

        private IEnumerable<Point2D> GenerateNodes
        {
            get
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
        }

        private IEnumerable<Element> GenerateElements
        {
            get
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

        private IEnumerable<NodeIndexes> ElementNodeIndexes
        {
            get
            {
                var nodesPerXAxis = _splitParameter.XSteps + 1;
                for (var i = 0; i < _splitParameter.YSteps; i++)
                {
                    var leftBottomStartIndex = i * nodesPerXAxis;
                    var leftTopStartIndex = leftBottomStartIndex + nodesPerXAxis;

                    for (var j = 0; j < _splitParameter.XSteps; j++)
                    {
                        var leftBottomIndex = leftBottomStartIndex + j;
                        var leftTopIndex = leftTopStartIndex + j;

                        yield return new NodeIndexes(
                            leftBottomIndex,
                            leftBottomIndex + 1,
                            leftTopIndex,
                            leftTopIndex + 1
                        );
                    }
                }
            }
        }
    }
}
