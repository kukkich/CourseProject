using BoundaryProblem.Geometry;

namespace CourseProject.Tests.Geometry
{
    internal class ElementTests
    {
        private Element _element;

        [SetUp]
        public void Setup()
        {
            _element = new(new Rectangle(
                new(1, 1),
                new(2, 1),
                new(1, 2),
                new(2, 2)
            ));
        }

        [Test]
        public void EqualTest()
        {
            Element sameElement = new(new Rectangle(
                new(1, 1),
                new(2, 1),
                new(1, 2),
                new(2, 2)
            ));

            Assert.That(_element, Is.EqualTo(sameElement));
        }

        [Test]
        public void NotEqualTest()
        {
            Element notSameElement = new(new Rectangle(
                new(1, 1),
                new(3, 1),
                new(1, 3),
                new(3, 3)
            ));

            Assert.That(_element, Is.Not.EqualTo(notSameElement));
        }

        [Test]
        public void LocalNodesTest()
        {
            List<Point2D> localNodes = new()
            {
                new (1, 1),
                new (2, 1),
                new (1, 2),
                new (2, 2),
            };

            Assert.That(localNodes.SequenceEqual(_element.LocalNodes), Is.True);
        }
    }
}
