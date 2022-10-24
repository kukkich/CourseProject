using BoundaryProblem.Calculus;
using BoundaryProblem.Geometry;

namespace CourseProject.Tests.Calculus
{
    internal class MatrixBuilderTests
    {
        private MatrixBuilder _matrixBuilder;
        private Point2D[] _points;
        private Element[] _elems;

        [SetUp]
        public void Setup()
        {
            _matrixBuilder = new();
            _points = Array.Empty<Point2D>();
            _elems = Array.Empty<Element>();
        }

        private Grid Splited_3X_To_2Y_Grid => new(_points, _elems,
            new NodeIndexes[]
            {
                new(0, 1, 4, 5),
                new(1, 2, 5, 6),
                new(2, 3, 6, 7),
                new(4, 5, 8, 9),
                new(5, 6, 9, 10),
                new(6, 7, 10, 11),
            });

        [Test]
        public void TestRowIndexes_Split_3X_To_2Y()
        {
            var grid = Splited_3X_To_2Y_Grid;

            var rowIndexes = _matrixBuilder.FromGrid(grid)
                .RowIndexes
                .ToArray();

            var expected = new[] { 0, 1, 2, 3, 5, 9, 13, 16, 18, 22, 26, 29 };

            Assert.That(expected.SequenceEqual(rowIndexes), Is.True);
        }

        [Test]
        public void TestColumnIndexes_Split_3X_To_2Y()
        {
            var grid = Splited_3X_To_2Y_Grid;

            var rowIndexes = _matrixBuilder.FromGrid(grid)
                .ColumnIndexes
                .ToArray();

            var expected = new[]
            {
                0,
                1,
                2,
                0, 1,
                0, 1, 2, 4,
                1, 2, 3, 5,
                2, 3, 6,
                4, 5,
                4, 5, 6, 8,
                5, 6, 7, 9,
                6, 7, 10
            };

            Assert.That(expected.SequenceEqual(rowIndexes), Is.True);
        }

        private Grid Splited_2X_To_2Y_Grid => new(_points, _elems,
            new List<NodeIndexes>
            {
                new(0, 1, 3, 4),
                new(1, 2, 4, 5),
                new(3, 4, 6, 7),
                new(4, 5, 7, 8)
            });

        [Test]
        public void TestRowIndexes_Split_2X_To_2Y()
        {
            var grid = Splited_2X_To_2Y_Grid;

            var rowIndexes = _matrixBuilder.FromGrid(grid)
                .RowIndexes
                .ToArray();

            var expected = new[] { 0, 1, 2, 4, 8, 10, 12, 16, 19 };

            Assert.That(expected.SequenceEqual(rowIndexes), Is.True);
        }

        [Test]
        public void TestColumnIndexes_Split_2X_To_2Y()
        {
            var grid = Splited_2X_To_2Y_Grid;

            var rowIndexes = _matrixBuilder.FromGrid(grid)
                .ColumnIndexes
                .ToArray();

            var expected = new[]
            {
                0,
                1,
                0, 1,
                0, 1, 2, 3,
                1, 2,
                3, 4,
                3, 4, 5, 6,
                4, 5, 7
            };

            Assert.That(expected.SequenceEqual(rowIndexes), Is.True);
        }

        private Grid Splited_2X_To_4Y_Grid => new(_points, _elems,
            new List<NodeIndexes>
            {
                new(0, 1, 3, 4),
                new(1, 2, 4, 5),
                new(3, 4, 6, 7),
                new(4, 5, 7, 8),
                new(6, 7, 9, 10),
                new(7, 8, 10, 11),
                new(9, 10, 12, 13),
                new(10, 11, 13, 14),
            });

        [Test]
        public void TestRowIndexes_Split_2X_To_4Y()
        {
            var grid = Splited_2X_To_4Y_Grid;

            var rowIndexes = _matrixBuilder.FromGrid(grid)
                .RowIndexes
                .ToArray();

            var expected = new[] { 0, 1, 2, 4, 8, 11, 13, 17, 20, 22, 26, 29, 31, 35, 38 };

            Assert.That(expected.SequenceEqual(rowIndexes), Is.True);
        }

        [Test]
        public void TestColumnIndexes_Split_2X_To_4Y()
        {
            var grid = Splited_2X_To_4Y_Grid;

            var rowIndexes = _matrixBuilder.FromGrid(grid)
                .ColumnIndexes
                .ToArray();

            var expected = new[]
            {
                0,
                1,
                0, 1,
                0, 1, 2, 3,
                1, 2, 4,
                3, 4,
                3, 4, 5, 6,
                4, 5, 7,
                6, 7,
                6, 7, 8, 9,
                7, 8, 10,
                9, 10,
                9, 10, 11, 12,
                10, 11, 13
            };

            Assert.That(expected.SequenceEqual(rowIndexes), Is.True);
        }
    }
}
