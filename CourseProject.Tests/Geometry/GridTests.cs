using BoundaryProblem.Geometry;

namespace CourseProject.Tests.Geometry
{
    internal class GridTests
    {
        [Test]
        public void TestNodeIndexes_Split_1X_To_1Y()
        {
            AxisSplitParameter splitting = new(1, 1);
            Grid grid = new(default, default, splitting);

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
            Grid grid = new(default, default, splitting);

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
            Grid grid = new(default, default, splitting);

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
            Grid grid = new(default, default, splitting);

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
            Grid grid = new(default, default, splitting);

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
