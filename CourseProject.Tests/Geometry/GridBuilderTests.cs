using BoundaryProblem.Geometry;
using Rectangle = BoundaryProblem.Geometry.Rectangle;

namespace CourseProject.Tests.Geometry
{
    internal class GridBuilderTests
    {
        private GridBuilder _gridBuilder;
        private Rectangle _rect;

        [SetUp]
        public void Setup()
        {
            _rect = new Rectangle(
                new Point2D(1, 1),
                new Point2D(3, 1),
                new Point2D(1, 7),
                new Point2D(3, 7)
            );
            
            _gridBuilder = new GridBuilder(_rect);
        }

        [Test]
        public void TestGridNodes()
        {
            var expected = new List<Point2D>
            {
                new(1, 1),
                new(2, 1),
                new(3, 1),

                new(1, 4),
                new(2, 4),
                new(3, 4),

                new(1, 7),
                new(2, 7),
                new(3, 7),
            };

            var computedNodes = _gridBuilder.Build(new AxisSplitParameter(2, 2)).Nodes;

            Assert.That(expected.SequenceEqual(computedNodes), Is.True);
        }

        [Test]
        public void TestGridElements()
        {
            var expected = new List<Element>
            {
                new(new Rectangle(
                    new Point2D(1, 1), new Point2D(2, 1), new Point2D(1, 4), new Point2D(2, 4)
                )),
                new(new Rectangle(
                    new Point2D(2, 1), new Point2D(3, 1), new Point2D(2, 4), new Point2D(3, 4)
                )),
                new(new Rectangle(
                    new Point2D(1, 4), new Point2D(2, 4), new Point2D(1, 7), new Point2D(2, 7)
                )),
                new(new Rectangle(
                    new Point2D(2, 4), new Point2D(3, 4), new Point2D(2, 7), new Point2D(3, 7)
                ))
            };

            var computedGrid = _gridBuilder.Build(new AxisSplitParameter(2, 2)).Elements;

            Assert.That(expected.SequenceEqual(computedGrid), Is.True);
        }
    }
}
