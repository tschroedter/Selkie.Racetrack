using System.Diagnostics.CodeAnalysis;
using Core2.Selkie.Geometry;
using Core2.Selkie.Geometry.Primitives;
using Core2.Selkie.Geometry.Shapes;
using NUnit.Framework;

namespace Core2.Selkie.Racetrack.Tests.Turn.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class TurnCircleTests
    {
        [SetUp]
        public void Setup()
        {
            m_Circle = new Circle(2.0,
                                  3.0,
                                  4.0);

            m_TurnCircle = new TurnCircle(m_Circle,
                                          Constants.CircleSide.Starboard,
                                          Constants.CircleOrigin.Start,
                                          Constants.TurnDirection.Clockwise);
        }

        private Circle m_Circle;
        private TurnCircle m_TurnCircle;

        [Test]
        public void CentrePointIsUnknownForUnknownTest()
        {
            Point centrePoint = TurnCircle.Unknown.CentrePoint;

            Assert.True(centrePoint.IsUnknown);
        }

        [Test]
        public void CentrePointTest()
        {
            Assert.AreEqual(m_Circle.CentrePoint,
                            m_TurnCircle.CentrePoint);
        }

        [Test]
        public void CircleIsUnknownForUnknownTest()
        {
            ICircle circle = TurnCircle.Unknown.Circle;

            Assert.True(circle.IsUnknown);
        }

        [Test]
        public void CircleTest()
        {
            Assert.AreEqual(m_Circle,
                            m_TurnCircle.Circle);
        }

        [Test]
        public void DirectionTest()
        {
            Assert.AreEqual(Constants.TurnDirection.Clockwise,
                            m_TurnCircle.TurnDirection);
        }

        [Test]
        public void EqualsOperatorReturnTrueForSameValuesTest()
        {
            var other = new TurnCircle(m_Circle,
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Clockwise);

            Assert.True(m_TurnCircle == other);
        }

        [Test]
        public void EqualsReturnFalseForDifferentCentrePointTest()
        {
            var other = new TurnCircle(new Circle(new Point(-10.0,
                                                            -10.0),
                                                  5.0),
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Clockwise);

            Assert.False(m_TurnCircle.Equals(other));
        }

        [Test]
        public void EqualsReturnFalseForDifferentOriginTest()
        {
            var other = new TurnCircle(new Circle(new Point(-10.0,
                                                            -10.0),
                                                  5.0),
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Finish,
                                       Constants.TurnDirection.Clockwise);

            Assert.False(m_TurnCircle.Equals(other));
        }

        [Test]
        public void EqualsReturnFalseForDifferentSideTest()
        {
            var other = new TurnCircle(new Circle(new Point(-10.0,
                                                            -10.0),
                                                  5.0),
                                       Constants.CircleSide.Port,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Clockwise);

            Assert.False(m_TurnCircle.Equals(other));
        }

        [Test]
        public void EqualsReturnFalseForDifferentTurnDirectionTest()
        {
            var other = new TurnCircle(new Circle(new Point(-10.0,
                                                            -10.0),
                                                  5.0),
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Counterclockwise);

            Assert.False(m_TurnCircle.Equals(other));
        }

        [Test]
        public void EqualsReturnFalseForNullTest()
        {
            Assert.False(m_TurnCircle.Equals(null));
        }

        [Test]
        public void EqualsReturnFalseForOtherClassTest()
        {
            Assert.False(m_TurnCircle.Equals(new object()));
        }

        [Test]
        public void EqualsReturnTrueForSameTest()
        {
            Assert.True(m_TurnCircle.Equals(m_TurnCircle));
        }

        [Test]
        public void EqualsReturnTrueForSameValuesTest()
        {
            var other = new TurnCircle(m_Circle,
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Clockwise);

            Assert.True(m_TurnCircle.Equals(other));
        }

        [Test]
        public void GetHashCodeTest()
        {
            Assert.DoesNotThrow(() => m_TurnCircle.GetHashCode());
        }

        [Test]
        public void NotEqualsOperatorReturnTrueForSameValuesTest()
        {
            var other = new TurnCircle(m_Circle,
                                       Constants.CircleSide.Starboard,
                                       Constants.CircleOrigin.Start,
                                       Constants.TurnDirection.Counterclockwise);

            Assert.True(m_TurnCircle != other);
        }

        [Test]
        public void OriginIsUnknownForUnknownTest()
        {
            Assert.AreEqual(Constants.CircleOrigin.Unknown,
                            TurnCircle.Unknown.Origin);
        }

        [Test]
        public void OriginTest()
        {
            Assert.AreEqual(Constants.CircleOrigin.Start,
                            m_TurnCircle.Origin);
        }

        [Test]
        public void RadiusIsUnknownForUnknownTest()
        {
            Distance distance = TurnCircle.Unknown.Radius;

            Assert.True(distance.IsUnknown);
        }

        [Test]
        public void RadiusTest()
        {
            Assert.AreEqual(m_Circle.Radius,
                            m_TurnCircle.Radius.Length);
        }

        [Test]
        public void SideIsUnknownForUnknownTest()
        {
            Assert.AreEqual(Constants.CircleSide.Unknown,
                            TurnCircle.Unknown.Side);
        }

        [Test]
        public void SideTest()
        {
            Assert.AreEqual(Constants.CircleSide.Starboard,
                            m_TurnCircle.Side);
        }

        [Test]
        public void UnknownDefaultTest()
        {
            Assert.False(m_TurnCircle.IsUnknown);
        }

        [Test]
        public void UnknownIsTrueForUnknownTest()
        {
            Assert.True(TurnCircle.Unknown.IsUnknown);
        }
    }
}