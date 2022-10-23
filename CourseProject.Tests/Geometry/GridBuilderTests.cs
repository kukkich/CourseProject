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

        [Test]
        public void TestNodeIndexes_Split_1X_To_1Y()
        {
            AxisSplitParameter splitting = new(1, 1);
            var grid = _gridBuilder.Build(splitting);

            var expected = new List<NodeIndexes>
            {
                new(0, 1, 2, 3)
            };

            Assert.That(expected.SequenceEqual(grid.ElementNodeIndexes), Is.True);
        }

        [Test]
        public void TestNodeIndexes_Split_2X_To_2Y()
        {
            AxisSplitParameter splitting = new(2, 2);
            var grid = _gridBuilder.Build(splitting);

            var expected = new List<NodeIndexes>
            {
                new(0, 1, 3, 4),
                new(1, 2, 4, 5),
                new(3, 4, 6, 7),
                new(4, 5, 7, 8)
            };

            Assert.That(expected.SequenceEqual(grid.ElementNodeIndexes), Is.True);
        }

        [Test]
        public void TestNodeIndexes_Split_3X_To_2Y()
        {
            AxisSplitParameter splitting = new(3, 2);
            var grid = _gridBuilder.Build(splitting);

            var expected = new List<NodeIndexes>
            {
                new(0, 1, 4, 5),
                new(1, 2, 5, 6),
                new(2, 3, 6, 7),
                new(4, 5, 8, 9),
                new(5, 6, 9, 10),
                new(6, 7, 10, 11),
            };

            Assert.That(expected.SequenceEqual(grid.ElementNodeIndexes), Is.True);
        }

        [Test]
        public void TestNodeIndexes_Split_2X_To_4Y()
        {
            AxisSplitParameter splitting = new(2, 4);
            var grid = _gridBuilder.Build(splitting);

            var expected = new List<NodeIndexes>
            {
                new(0, 1, 3, 4),
                new(1, 2, 4, 5),
                new(3, 4, 6, 7),
                new(4, 5, 7, 8),
                new(6, 7, 9, 10),
                new(7, 8, 10, 11),
                new(9, 10, 12, 13),
                new(10, 11, 13, 14),
            };

            Assert.That(expected.SequenceEqual(grid.ElementNodeIndexes), Is.True);
        }

        [Test]
        public void TestNodeIndexes_Split_4X_To_2Y()
        {
            AxisSplitParameter splitting = new(4, 2);
            var grid = _gridBuilder.Build(splitting);

            var expected = new List<NodeIndexes>
            {
                new(0, 1, 5, 6),
                new(1, 2, 6, 7),
                new(2, 3, 7, 8),
                new(3, 4, 8, 9),
                new(5, 6, 10, 11),
                new(6, 7, 11, 12),
                new(7, 8, 12, 13),
                new(8, 9, 13, 14),
            };

            Assert.That(expected.SequenceEqual(grid.ElementNodeIndexes), Is.True);
        }
    }
}
